using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using WiFiLocationServer.Models;

namespace WiFiLocationServer.DB
{
    /// <summary>
    /// 数据访问类:手机型号表
    /// </summary>
    public class data_test
    {
        #region 公用方法=================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_sensor_wifi_test");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Models.data_test model)
        {           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_sensor_wifi_test(coord,memory,addtime,rssi,flag)");
            strSql.Append(" values (@coord,@memory,@addtime,@rssi,@flag)");
            SqlParameter[] parameters = {
                    new SqlParameter("@coord", SqlDbType.VarChar),
                    new SqlParameter("@memory", SqlDbType.VarChar),
                    new SqlParameter("@addtime", SqlDbType.VarChar,50),
                    new SqlParameter("@rssi", SqlDbType.Int,4),
                    new SqlParameter("@flag", SqlDbType.Int,4)};
            parameters[0].Value = model.coord;
            parameters[1].Value = model.memory;
            parameters[2].Value = model.addtime;
            parameters[3].Value = model.rssi;
            parameters[4].Value = model.flag;

            object obj = DbHelperSQL.ExecuteSql(strSql.ToString(),parameters); 
            model.id = Convert.ToInt32(obj);

            return model.id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Models.data_test model)
        {
         
            return true;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_sensor_wifi_test ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            int rowsAffected = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Models.data_test GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,coord,actual_coord,memory,addtime,rssi,flag");
            strSql.Append(" from tb_sensor_wifi_test");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Models.data_test model = new Models.data_test();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion



        #region 私有方法=================================
        ///<summary>
        /// 将对象转换为实体
        /// </summary>
        private Models.data_test DataRowToModel(DataRow row)
        {
            Models.data_test model = new Models.data_test();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["coord"] != null && row["coord"].ToString() != "")
                {
                    model.coord = row["coord"].ToString();
                }
                if (row["actual_coord"] != null && row["actual_coord"].ToString() != "")
                {
                    model.coord = row["actual_coord"].ToString();
                }
                if (row["memory"] != null && row["memory"].ToString() != "")
                {
                    model.memory = row["memory"].ToString();
                }
                if (row["addtime"] != null && row["addtime"].ToString() != "")
                {
                    model.addtime = row["addtime"].ToString();
                }
                if (row["rssi"] != null && row["rssi"].ToString() != "")
                {
                    model.rssi = Int32.Parse(row["rssi"].ToString());
                }
                if (row["flag"] != null && row["flag"].ToString() != "")
                {
                    model.flag = Int32.Parse(row["flag"].ToString());
                }

            }
            return model;
        }

        ///<summary>
        /// 获取所有备注列表
        /// </summary> 
        public DataSet GetMemoryList()
        {
            string sql = "select id,memory from tb_sensor_wifi_test where flag = 1";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }
        #endregion


    }
}