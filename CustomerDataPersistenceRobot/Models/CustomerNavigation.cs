using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerDataPersistenceRobot.Models
{
    public class CustomerNavigation
    {
        public Int32 Id { get; set; }
        public DateTime Date { get; set; }
        public String IP { get; set; }
        public String Params { get; set; }
        public String PageTitle { get; set; }
        public String Browser { get; set; }

    }
}
