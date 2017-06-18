using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Text;
using WiFiLocationServer.Models;

namespace WiFiLocationServer.Controllers
{
    public class RoomController : ApiController
    {
        DB.room db = new DB.room();
        // GET api/values/
        [HttpGet]
        public DataSet GetAllRoom()
        {
            DataSet ds = db.GetRoomList();
            return ds;
        }
        
        // GET api/values/id
        [HttpGet]
        public DataSet GetRoomById(int id)
        {
            DataSet ds =  db.GetRoomDetial(id);
            return ds;
        }

        // GET api/values?building=&floor=
        [HttpGet]
        public DataSet GetRoomByFloor(int building, int floor)
        {
            string where = " building = " + building + " and floor_num = " + floor;
            return db.GetRoomBySome(where);
        }

    
    }
}
