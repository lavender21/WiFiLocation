﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WiFiLocationServer.DB;

namespace WiFiLocationServer.Controllers
{
    public class FingerprintController : ApiController
    {
        DB.fingerprint db_fingerprint = new fingerprint();
        DB.rssi db_rssi = new rssi();

        // POST /api/Fingerprint/
        public HttpResponseMessage Post([FromBody]JObject value)
        {
            var response = Request.CreateResponse();
            int flag = 0;   // 0 处理成功 1 部分rssi处理成功 2 全部rssi处理失败 3 coord和rssi全部失败
            // 插入坐标相关信息
            Models.Fingerprint model_finger = new Models.Fingerprint();
            model_finger.coord = value["coord"].ToString();
            model_finger.memory = value["memory"].ToString();
            model_finger.addtime = value["addtime"].ToString();
            model_finger.flag = Int32.Parse(value["flag"].ToString());
            int coord_id = db_fingerprint.Add(model_finger);

            // 插入各个AP的rssi数据
            if (coord_id > 0)
            {
                Models.Rssi model_rssi = new Models.Rssi();
                model_rssi.coord_id = coord_id;
                model_rssi.mobile_id = Int32.Parse(value["mobile_id"].ToString());
                JToken ap = value["ap"];
                if (ap.Count() > 0)
                {
                    IEnumerable<JToken> temp = ap.Values();
                    foreach (var item in temp)
                    {
                        model_rssi.rssi = Int32.Parse(item.First().ToString());
                        model_rssi.mac = item.Last().ToString();
                        if(!db_rssi.Add(model_rssi))
                        {
                            flag = 1;
                            continue;
                        }
                    }
                }
                else { flag = 2; }
            }
            else{ flag = 3;}

            // 向前台返回响应数据
            switch (flag) {
                case 0:             // 全部处理成功
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent("{\"message\":\"上传成功\"}", Encoding.Unicode);
                    break;
                case 1:
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent("{\"message\":\"部分rssi上传成功\"}", Encoding.Unicode);
                    break;
                case 2:
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent("{\"message\":\"coord数据上传成功，rssi数据上传失败！\"}", Encoding.Unicode);
                    break;
                case 3:
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent("{\"message\":\"全部上传失败！\"}", Encoding.Unicode);
                    break;
            }
           

            return response;
        }
    }
}