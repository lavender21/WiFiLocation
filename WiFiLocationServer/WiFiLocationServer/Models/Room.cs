using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiFiLocationServer.Models
{
    public class Room
    {
        public int id { set; get; }
        public string room_name { set; get; }
        public string floor_num { set; get; }
        public string left_up { set; get; }
        public string right_up { set; get; }
        public string left_down { set; get; }
        public string right_down { set; get; }

    }
}