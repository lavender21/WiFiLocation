using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using WiFiLocationServer.DB;

namespace WiFiLocationServer.Controllers
{
    public class LocationLogController : ApiController
    {
        //GET /api/LocationLog/
        [HttpGet]
        public DataSet GetLogList(int room, int mobile, int algorithm)
        {
            location_log logDb = new location_log();
            return logDb.GetlocationLogList(room,mobile,algorithm);
        }
    }
}
