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
        public Models.Room GetRoomById(int id)
        {
            Models.Room model =  db.GetModel(id);
            var response = Request.CreateResponse();
            if (model != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(model.ToString(), Encoding.Unicode);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("{\"message\":\"无该资源\"}", Encoding.Unicode);
            }
            return model;
        }
    }
}
