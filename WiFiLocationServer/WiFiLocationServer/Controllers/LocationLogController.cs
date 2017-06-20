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
            string where = "room_id = " + room + " and mobile_id = " + mobile + "and location_algorithm = " + algorithm + "and  memory is null and flag = 4";
            location_log logDb = new location_log();
            return logDb.GetlocationLogList(where);
        }
    }
}
