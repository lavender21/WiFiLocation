using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiFiLocationServer.Models
{
    public class Fingerprint
    {
        public int id { set; get; }
        public string coord { set; get; }
        public string addtime { set; get; }
        public int flag { set; get; }
        public string memory { set; get; }
    }
}