using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WiFiLocationServer.Models
{
    public class mobile_model
    {
        public mobile_model()
        { }
        #region Model
        private int _id;
        private string _model_name;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 手机型号
        /// </summary>
        public string model_name
        {
            set { model_name = value; }
            get { return model_name; }
        }
        #endregion
    }
}