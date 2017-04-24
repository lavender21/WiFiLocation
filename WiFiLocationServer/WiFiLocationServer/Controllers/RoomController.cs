using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace WiFiLocationServer.Controllers
{
    public class RoomController : ApiController
    {
        DB.room db = new DB.room();
        // GET api/values/5
        [HttpGet]
        public DataSet GetAllRoom()
        {
            DataSet ds = db.GetRoomList();
            return ds;
        }
    }
}
