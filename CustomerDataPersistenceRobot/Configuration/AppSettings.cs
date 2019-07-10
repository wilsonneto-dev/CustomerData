using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerDataPersistenceRobot.Configuration
{
    class AppSettings
    {
        public string RabbitMQConnectionString { get; set; }
        public string SqlConnectionString { get; set; }
        public string NoSqlUser { get; set; }
        public string NoSqlPass { get; set; }
        public string NoSqlBucket { get; set; }

    }
}
