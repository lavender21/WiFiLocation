using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WiFiLocationServer.DB;
using System.Data;
using System.Text.RegularExpressions;

namespace WiFiLocationServer.Models
{
    public class Location
    {
        public string KNNalgorithm(int k, Dictionary<string, int> rssiList, Dictionary<int, DataTable> coordDictionary)
        {
            string result = "";
            var DistanceList = new Dictionary<int, float>();  // {coord:rssi}
            foreach (var item in coordDictionary)
            {
                float sum = 0;
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
                DistanceList.Add(item.Key, sum/number);
            }
            var sortedList = from pair in DistanceList
                             orderby pair.Value ascending
                             select pair;
            string idList = "";
            foreach (var item in sortedList)
            {
                if (k-- == 0)
                {
                    break;
                }
                idList += item.Key + ",";
            }
            idList = idList.Substring(0, idList.Length - 1);
            result = calculateCoord(k, idList);
            return result;
        }

        public string MHJalgorithm(int k, Dictionary<string, int> rssiList, Dictionary<int, DataTable> coordDictionary)
        {
            Dictionary<int, double> collectionSi = new Dictionary<int, double>();
            foreach (var item in coordDictionary)
            {
                DataTable dt = item.Value;
                Dictionary<string,List<int>> list = convertList(rssiList, dt);
                double innerProduct = calculateVectorInnerProduct(list["T"], list["F"]);
                double Tmod = calculateVectorMod(list["T"]);
                double Fmod = calculateVectorMod(list["F"]);
                double Si = innerProduct / (Tmod * Fmod);
                collectionSi.Add(item.Key, Si);
            }

            var sortedcollectionSi = from pair in collectionSi
                                     orderby pair.Value descending
                                     select pair;
            string idList = "";
            foreach (var item in sortedcollectionSi)
            {
                if (k-- == 0)
                {
                    break;
                }
                idList += item.Key + ",";
            }
            idList = idList.Substring(0, idList.Length - 1);
            string result = calculateCoord(k, idList);
            return result;
        }

        public double calculateVectorMod(List<int> vector)
        {
            double result = 0;
            foreach (var item in vector)
            {
                result += item * item;
            }
            result = Math.Round(Math.Sqrt(result),4);
            return result;
        }

        public double calculateVectorInnerProduct(List<int> T1, List<int> T2)
        {
            double result = 0;
            int length = T1.Count > T2.Count ? T2.Count : T1.Count;
            for (int i = 0; i < length; i++)
            {
                result += T1[i] * T2[i];
            }
            return result;
        }

        public Dictionary<string,List<int>> convertList(Dictionary<string, int> rssiList, DataTable dt)
        {
            List<int> T = new List<int>();
            List<int> F = new List<int>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (rssiList.ContainsKey(dt.Rows[i]["mac"].ToString()))
                {
                    T.Add(rssiList[dt.Rows[i]["mac"].ToString()]);
                    F.Add(int.Parse(dt.Rows[i]["rssi"].ToString()));
                }
            }
            Dictionary<string, List<int>> result = new Dictionary<string, List<int>>();
            result.Add("T", T);
            result.Add("F", F);
            return result;
        }

        public string calculateCoord(int k, string idList)
        {
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
            for (int j = 0; j < coordLength; j++)
            {
                Regex reg = new Regex("[,，]");
                var coord = reg.Split(coordDt.Rows[j]["coord"].ToString());
                xCoord += int.Parse(coord[0]);
                yCoord += int.Parse(coord[1]);
            }
            return Convert.ToString(xCoord / coordLength) + ',' + (yCoord / coordLength);
        }
    }
}