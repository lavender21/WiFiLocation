using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WiFiLocationServer.Models;
using System.Collections.Generic;
using System.Data;

namespace WiFiLocationServer.测试
{
    [TestClass]
    public class LocationTest
    {
        [TestMethod]
        public void TestCalculateVectorMod()
        {
            Location location = new Location();
   
            List<int> T = new List<int>();
            T.Add(-45);
            T.Add(-56);
            T.Add(-76);
            T.Add(-87);
            double expect = 136.0368;

            double result = location.calculateVectorMod(T);

            Assert.AreEqual<double>(expect, result);
        }

        [TestMethod]
        public void TestCalculateVectorInnerProduct()
        {
            Location location = new Location();

            List<int> T1 = new List<int>();
            T1.Add(-45);
            T1.Add(-56);
            T1.Add(-76);
            T1.Add(-87);
            List<int> T2 = new List<int>();
            T2.Add(-56);
            T2.Add(-63);
            T2.Add(-74);
            double expect = 11672;

            double result = location.calculateVectorInnerProduct(T1, T2);

            Assert.AreEqual<double>(expect, result);

        }

        [TestMethod]
        public void TestConvertList()
        {
            Location location = new Location();
            Dictionary<string,int> rssiList = new Dictionary<string,int>();
            rssiList.Add("0a:69:6c:51:36:80", -40);
            rssiList.Add("0a:69:6c:51:36:7f", -50);
            rssiList.Add("0a:69:6c:51:3d:94", -60);
            rssiList.Add("06:69:6c:50:b8:49", -70);

            DataTable dt = new DataTable();
            DataRow row;
            DataColumn col;

            col = new DataColumn();
            col.DataType = System.Type.GetType("System.Int32");
            col.ColumnName = "rssi";
            col.ReadOnly = true;
            dt.Columns.Add(col);
            col = new DataColumn();
            col.DataType = System.Type.GetType("System.String");
            col.ColumnName = "mac";
            col.ReadOnly = true;
            col.Unique = true;
            dt.Columns.Add(col);


            row = dt.NewRow();
            row["rssi"] = -50;
            row["mac"] = "0a:69:6c:51:36:80";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["rssi"] = -60;
            row["mac"] = "0a:69:6c:51:36:7f";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["rssi"] = -70;
            row["mac"] = "0a:69:6c:51:3d:94";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["rssi"] = -80;
            row["mac"] = "0a:69:6c:51:35:eb";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["rssi"] = -60;
            row["mac"] = "06:69:6c:50:b8:49";
            dt.Rows.Add(row);

            Dictionary<string, List<int>> expect = new Dictionary<string, List<int>>();
            List<int> T = new List<int>();
            List<int> F = new List<int>();
            T.Add(-40);
            F.Add(-50);
            T.Add(-50);
            F.Add(-60);
            T.Add(-60);
            F.Add(-70);
            T.Add(-70);
            F.Add(-60);
            expect.Add("T", T);
            expect.Add("F", F);

            Dictionary<string, List<int>> result = location.convertList(rssiList, dt);

            Assert.IsTrue(expect["T"].Count == result["T"].Count);
            Assert.IsTrue(expect["F"].Count == result["F"].Count);
            Assert.IsTrue(expect["T"][0] == result["T"][0]);
            Assert.IsTrue(expect["T"][1] == result["T"][1]);
            Assert.IsTrue(expect["T"][2] == result["T"][2]);
            Assert.IsTrue(expect["T"][3] == result["T"][3]);
            Assert.IsTrue(expect["F"][0] == result["F"][0]);
            Assert.IsTrue(expect["F"][1] == result["F"][1]);
            Assert.IsTrue(expect["F"][2] == result["F"][2]);
            Assert.IsTrue(expect["F"][3] == result["F"][3]);
        }
    }
}
