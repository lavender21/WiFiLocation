using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiFiLocationServer.Models
{
    public class Rssi
    {
        public int id { set; get; }
        public string mac { set; get; }
        public int rssi { set; get; }
        public int coord_id { set; get; }
        public int mobile_id { set; get; }
        public int room_id { set; get; }
    }
}