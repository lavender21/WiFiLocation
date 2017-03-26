using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiFiLocationServer.Models
{
    public class mobile_model
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int id
        {
            set;
            get;
        }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string model_name
        {
            set;
            get;
        }
    }
}