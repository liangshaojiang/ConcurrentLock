using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApi.Models
{
    
    public class Customer
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
          
        public string name { get; set; }

        public string fingerprint { get; set; }


    }
}
