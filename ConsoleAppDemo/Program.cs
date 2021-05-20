using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppDemo
{
    class Program
    {
        static string url = "https://localhost:5001/WeatherForecast";


        static void Main(string[] args)
        {


            for (int i = 0; i < 20; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(PostHttp));
                Thread thread2 = new Thread(new ParameterizedThreadStart(PostHttp));
                Thread thread3 = new Thread(new ParameterizedThreadStart(PostHttp));


                thread.Start(i);
                thread2.Start(i);
                thread3.Start(i);


                //Thread thread4 = new Thread(new ParameterizedThreadStart(PostHttp));
                //Thread thread5 = new Thread(new ParameterizedThreadStart(PostHttp));
                //Thread thread6 = new Thread(new ParameterizedThreadStart(PostHttp));


                //thread4.Start(i);
                //thread5.Start(i);
                //thread6.Start(i);

                //同步执行
                //PostHttp(i);


            }




            Console.WriteLine("执行完成！");
            Console.Write("输入任意键退出：");
            Console.ReadKey();

        }

        protected async static void PostHttp(object i)
        {
            //Task task = Task.Run(() =>
            //{
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + $"?id={i}");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            Console.WriteLine($"第{i}次请求结果{retString}");

            //});


        }
    }
}
