using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerDataQueryAPI.Configuration
{
    public class AppSettings
    {
        public string NoSqlUser { get; set; }
        public string NoSqlPass { get; set; }
        public string NoSqlBucket { get; set; }
    }
}