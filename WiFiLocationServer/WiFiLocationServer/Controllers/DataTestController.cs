using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WiFiLocationServer.Models;
using WiFiLocationServer.DB;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;


namespace WiFiLocationServer.Controllers
{
    public class DataTestController : ApiController
    {
        DB.data_test db = new DB.data_test();
        DB.rssi rssiDb = new DB.rssi();
        DB.fingerprint fingerDb = new DB.fingerprint();

        // Get /api/DataTest/id
        [HttpGet]
        public Models.data_test GetById(int id)
        {   
            Models.data_test model = db.GetModel(id);
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

        // POST /api/DataTest/
        public HttpResponseMessage Post([FromBody]Models.data_test model)
        {
            var response = Request.CreateResponse();

            if (model != null)
            {
                if (db.Add(model) > 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent("{\"message\":\"添加成功\"}", Encoding.UTF8);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent("{\"message\":\"发生未知错误\"}", Encoding.UTF8);
                }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("{\"message\":\"post数据为空\"}", Encoding.Unicode);
            }
            return response;
        }

        [HttpGet]
        public DataSet GetIdList()
        {
            return  db.GetMemoryList();
        }       

    }
}
