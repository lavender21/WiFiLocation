using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WiFiLocationServer.DAL;

namespace WiFiLocationServer.Controllers
{
    public class MobileModelController : ApiController
    {
        DAL.mobile_model dal = new mobile_model();
        // GET api/values
        public IEnumerable<Models.mobile_model> GetAllList()
        {
            
            return null;
        }

        // GET api/values/5
        [HttpGet]
        public Models.mobile_model GetById(int id)
        {
            Models.mobile_model model = dal.GetModel(id);
            return model;
        }

        // POST api/values
        [HttpPost]
        public HttpResponseMessage Post([FromBody]Models.mobile_model value)
        {
            int addId = dal.Add(value);
            var response = Request.CreateResponse();
            if (addId > 0)
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(addId+"",Encoding.Unicode);
                return response;
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("未知错误", Encoding.Unicode);
                return response;
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
