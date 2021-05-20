using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApi
{

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }

    }


    public class MyMiddleware
    {

        private readonly RequestDelegate _next;
        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            //  await _next(context);




            //启用读取request
            context.Request.EnableBuffering();

            //变量设置
            var request = context.Request;
            var response = context.Response;

            //请求body
            using var requestReader = new StreamReader(request.Body);
            var requestBody = await requestReader.ReadToEndAsync();
            request.Body.Position = 0;

            //设置stream存放ResponseBody
            var responseOriginalBody = response.Body;
            using var memStream = new MemoryStream();
            response.Body = memStream;

            // 执行其他中间件
            await _next(context);

            //处理执行其他中间件后的ResponseBody
            memStream.Position = 0;
            var responseReader = new StreamReader(memStream);
            var responseBody = await responseReader.ReadToEndAsync();
            memStream.Position = 0;
            await memStream.CopyToAsync(responseOriginalBody);
            response.Body = responseOriginalBody;


            if(context.Response.ContentType.ToLower().Equals(""))

            stopwatch.Stop();
            Console.WriteLine($"处理：{context.Request.Path}{context.Request.QueryString} 执行耗时：" + stopwatch.Elapsed.TotalSeconds + "s" + ",返回数据：" + responseBody);
        }

    }
}
