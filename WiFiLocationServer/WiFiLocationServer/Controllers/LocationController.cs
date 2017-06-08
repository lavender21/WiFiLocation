using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json.Linq;
using WiFiLocationServer.Models;
using WiFiLocationServer.DB;
using System.Text;

namespace WiFiLocationServer.Controllers
{
    public class LocationController : ApiController
    {  // POST /api/DataTest/
        public HttpResponseMessage Post([FromBody]JObject value)
        {
            var response = Request.CreateResponse();

            var room_id = value["room_id"].ToString();
            var mobile_id = value["mobile_id"].ToString();
            var algorithm = value["algorithm"].ToString();
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
                        rssiDictionary.Add(item["mac"].ToString(), int.Parse(item["rssi"].ToString()));
                    }
                    catch (Exception)
                    {
                        response.Content = new StringContent("请求数据错误！", Encoding.UTF8);
                        return response;
                    }
                }
            }
            macList = macList.Substring(0, macList.Length - 1);

            DB.rssi rssiDb = new rssi();
            string where = "";
            if (algorithm == "0")
            {
                where = "room_id = " + room_id + " and mobile_id = " + mobile_id + " and mac in(" + macList + ")";
            }
            else
            {
                where = "room_id = " + room_id + " and mobile_id = " + mobile_id;
            }
            DataSet ds = rssiDb.GetRssiList(where);
            var allRssiList = new Dictionary<int, DataTable>();
            if (ds != null)
            {
                allRssiList = convertAllRssiList(ds);
            }

            Location location = new Location();
            var result = "";
            if  (algorithm == "0")
            {
                result = location.KNNalgorithm(10, rssiDictionary, allRssiList);
            }
            else
            {
                result = location.MHJalgorithm(10, rssiDictionary, allRssiList);
            }
            DB.location_log locationLogDb = new DB.location_log();
            Models.location_log locationLogModel = new Models.location_log();
            locationLogModel.location_coord = result;
            locationLogModel.actual_coord = value["actual_coord"].ToString();
            locationLogModel.room_id = int.Parse(value["room_id"].ToString());
            locationLogModel.mobile_id = int.Parse(value["mobile_id"].ToString());
            locationLogModel.location_algorithm = int.Parse(algorithm);
            locationLogModel.location_time = GetTimeStamp();
            if (locationLogDb.Add(locationLogModel) > 0)
            {
                response.Content = new StringContent(result, Encoding.UTF8);
            }
            else
            {
                response.Content = new StringContent(result+"（定位记录添加失败）", Encoding.UTF8);
            }

            return response;
        }

        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        } 

        private Dictionary<int, DataTable> convertAllRssiList(DataSet ds)
        {
            var result = new Dictionary<int, DataTable>();
            var dt = ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable value = new DataTable();
                DataRow row;
                DataColumn col;

                col = new DataColumn();
                col.DataType = System.Type.GetType("System.Int32");
                col.ColumnName = "rssi";
                col.ReadOnly = true;
                value.Columns.Add(col);
                col = new DataColumn();
                col.DataType = System.Type.GetType("System.String");
                col.ColumnName = "mac";
                col.ReadOnly = true;
                col.Unique = true;
                value.Columns.Add(col);

                //value.Clear();
                int key = int.Parse(dt.Rows[i]["coord_id"].ToString());
                if (!result.ContainsKey(key))
                {
                    result.Add(key, value);
                }
                else
                {
                    value = result[key];
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