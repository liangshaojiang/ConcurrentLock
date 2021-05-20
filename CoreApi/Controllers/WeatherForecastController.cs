using CoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly object objlock = new object();

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost]
        public bool Edit()
        {
            //创建数据库对象
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=192.168.1.222;uid=sa;pwd=Txl123456;database=yncs",//连接符字串
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });


            /*
             
 
GO
CREATE TABLE[dbo].[Customer](

    [id][int] IDENTITY(1, 1) NOT NULL,

    [fingerprint] [nvarchar](4000) NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT[PK_Customer] PRIMARY KEY CLUSTERED
(

   [id] ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[ProductInfo](

    [Id][int] IDENTITY(1, 1) NOT NULL,

    [Name] [nvarchar](50) NULL,
	[Quantity] [int] NULL,
 CONSTRAINT[PK_ProductInfo] PRIMARY KEY CLUSTERED
(

   [Id] ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO
USE[master]
GO
ALTER DATABASE[yncs] SET READ_WRITE
GO
             
             */

            lock (objlock)
            {
                var pro = db.Queryable<ProductInfo>().First();

                if (pro.Quantity > 0)
                {
                    pro.Quantity--;
                    db.Updateable(pro).ExecuteCommand();
                    db.Insertable(new Customer { fingerprint = DateTime.Now.ToString(), name = pro.Quantity.ToString() + Request.QueryString }).ExecuteCommand();
                    return true;
                }
                else
                {
                    return false;

                }

            }






        }

    }
}
