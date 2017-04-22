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
                        rssiDictionary.Add(item["mac"].ToString(), int.Parse(item["rssi"].ToString()));
                    }
                    catch (Exception)
                    {
                        flag = 3;
                    }
                }
            }
            macList = macList.Substring(0, macList.Length - 1);

            DB.rssi rssiDb = new rssi();
            string where = "room_id = " + room_id + " and mobile_id = " + mobile_id + " and mac in(" + macList + ")";
            DataSet ds = rssiDb.GetRssiList(where);
            var allRssiList = new Dictionary<int, DataTable>();
            if (ds != null)
            {
                allRssiList = convertAllRssiList(ds);
            }

            Location location = new Location();
            var result = location.KNNalgorithm(5, rssiDictionary, allRssiList);
            response.Content = new StringContent("{\"message\":" + result + "}", Encoding.UTF8);
            return response;
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
                col.Unique = true;
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