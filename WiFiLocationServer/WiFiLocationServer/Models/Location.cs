using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiFiLocationServer.DB;
using System.Data;

namespace WiFiLocationServer.Models
{
    public class Location
    {
        public string KNNalgorithm(int k, Dictionary<string, int> rssiList, Dictionary<int, DataTable> coordDictionary)
        {
            string result = "";
            var DistanceList = new Dictionary<int, int>();  // {coord:rssi}
            foreach (var item in coordDictionary)
            {
                int sum = 0;
                DataTable dt = item.Value;
                int number = dt.Rows.Count;
                if (number == 0)
                {
                    break;
                }
                for (int i = 0; i < number; i++)
                {
                    int value = 0;
                    rssiList.TryGetValue(dt.Rows[i]["mac"].ToString(), out value);
                    sum += Math.Abs(int.Parse(dt.Rows[i]["rssi"].ToString()) - value);
                }

                DistanceList.Add(item.Key, sum / number);
            }

            var sortedDistanceList = from pair in DistanceList
                        orderby pair.Value ascending
                        select pair;

            string idList = "";
            foreach(var item in sortedDistanceList)
            {
                if (k-- == 0)
                {
                    break;
                }
                idList += item.Key+",";
            }
            idList = idList.Substring(0, idList.Length - 1);

            DB.fingerprint fingerDb = new fingerprint();
            DataSet ds = fingerDb.getCoordList(idList);
            DataTable coordDt = ds.Tables[0];
            int coordLength = coordDt.Rows.Count;
            if (coordLength == 0)
            {
                return "出现未知错误！";
            }

            int xCoord = 0;
            int yCoord = 0;
            string[] coord = new string[2];
            for (int j = 0; j < coordLength; j++)
            {
                coord = coordDt.Rows[j]["coord"].ToString().Split(',');
                xCoord += int.Parse(coord[0]);
                yCoord += int.Parse(coord[1]);
            }
            result = Convert.ToString(xCoord / coordLength) + ',' + (yCoord / coordLength);           
            return result;
        }
    }
}