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
        public DataSet GetById(int id)
        {
            //Models.data_test model = db.GetModel(id);
            //dataset ds = rssidb.getrssilist("room_id = " + id + " and mobile_id = 3 and mac in ('0a:69:6c:51:36:80','0a:69:6c:51:36:7f')");
            DataSet ds = fingerDb.getCoordList(Convert.ToString(id));
            var response = Request.CreateResponse();
            if (ds != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(ds.ToString(), Encoding.Unicode);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("{\"message\":\"无该资源\"}", Encoding.Unicode);
            }
            return ds;
        }

        // POST /api/DataTest/
        public HttpResponseMessage Post([FromBody]JObject value)
        {
            var response = Request.CreateResponse();
            int flag = 0;  // 1 success 2 error 3 input invalid

            var room_id = value["room_id"].ToString();
            var mobile_id = value["mobile_id"].ToString();
            JToken ap = new JObject();
            ap = value["ap"];
            var macList = "";
            var rssiDictionary = new Dictionary<string, int>();

            if (ap.Count() > 0)
            {
                IEnumerable<JToken> temp = ap.Values();
                foreach (var item in temp)
                {
                    try
                    {
                        macList += "'" + item["mac"].ToString() + "',";
                        rssiDictionary.Add(item["mac"].ToString(),int.Parse(item["rssi"].ToString()));
                    }
                    catch (Exception)
                    {
                        flag = 3;            
                    }
                }
            }
            macList = macList.Substring(0, macList.Length - 1);

            DB.rssi rssiDb = new rssi();
            string where = "room_id = " + room_id + " and mobile_id = "+ mobile_id +" and mac in("+ macList +")";
            DataSet ds = rssiDb.GetRssiList(where);
            var allRssiList = new Dictionary<int, DataTable>();
            if (ds != null)
            {
                allRssiList = convertAllRssiList(ds);
            }

            Location location = new Location();
            var result = location.KNNalgorithm(5, rssiDictionary, allRssiList);
            response.Content = new StringContent("{\"message\":"+ result +"}", Encoding.UTF8);
            return response;
        }

          private Dictionary<int, DataTable> convertAllRssiList(DataSet ds)
          {
              var result = new Dictionary<int, DataTable>();
              var dt = ds.Tables[0];
              DataTable value = new DataTable();
              DataRow row;
              DataColumn col;

              col = new DataColumn();
              col.DataType = System.Type.GetType("System.Int32");
              col.ColumnName = "rssi";
              col.ReadOnly = true;
              col.Unique = true;
              value.Columns.Add(col);
              col = new DataColumn();
              col.DataType = System.Type.GetType("System.String");
              col.ColumnName = "mac";
              col.ReadOnly = true;
              col.Unique = true;
              value.Columns.Add(col);
          
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  value.Clear();
                  int key = int.Parse(dt.Rows[i]["coord_id"].ToString());
                  if (result.ContainsKey(key))
                  {
                      result.TryGetValue(key, out value);
                  }
                  else
                  {
                      result.Add(key,value);
                  }
                  row = value.NewRow();
                  row["rssi"] = dt.Rows[i]["rssi"].ToString();
                  row["mac"] = dt.Rows[i]["mac"].ToString();
                  value.Rows.Add(row);
                  result[key] = value;
              }
              return result;
          }
    }
}
