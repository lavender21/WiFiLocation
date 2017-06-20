using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiFiLocationServer.Models
{
    public class location_log
    {
        public int id { set; get; }
        public string actual_coord { set; get; }
        public string location_coord { set; get; }
        public int room_id { set; get; }
        public int mobile_id { set; get; }
        public int location_algorithm { set; get; }
        public string memory { set; get; }
        public string location_time { set; get; }
        public int flag { set; get; }
    }
}