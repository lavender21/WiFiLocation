using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Text;
using WiFiLocationServer.Models;
using Newtonsoft.Json.Linq;


namespace WiFiLocationServer.Controllers
{
    public class LoginController : ApiController
    {
        DB.login db = new DB.login();

        // POST
        [HttpPost]
        public HttpResponseMessage Login([FromBody]JObject obj)
        {
            var response = Request.CreateResponse();
            var username = obj["username"].ToString();
            var password = obj["password"].ToString();
            if (db.ExistsByName(username))
            {
                Models.Login model = new Login();
                model = db.GetModelByName(username);
                if (model.password == password)
                {
                    response.Content = new StringContent("{\"status\":1,\"message\":\"登录成功\"}", Encoding.UTF8);
                }
                else
                {
                    response.Content = new StringContent("{\"status\":0,\"message\":\"密码错误！\"}", Encoding.UTF8);
                }
            }
            else
            {
                response.Content = new StringContent("{\"status\":0,\"message\":\"用户不存在！\"}", Encoding.UTF8);
            }
            return response;
        }
    }
}
