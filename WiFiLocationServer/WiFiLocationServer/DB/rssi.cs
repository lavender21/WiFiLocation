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
    public class rssi
    {
        #region 公用方法=================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_wifi_rssi");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Models.Rssi model)
        {           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_wifi_rssi(rssi,mobile_id,mac,coord_id)");
            strSql.Append(" values (@rssi,@mobile_id,@mac,@coord_id)");
            SqlParameter[] parameters = {
                    new SqlParameter("@rssi", SqlDbType.Int),
                    new SqlParameter("@mobile_id", SqlDbType.Int),
                    new SqlParameter("@mac", SqlDbType.VarChar,20),
                    new SqlParameter("@coord_id", SqlDbType.Int)};
            parameters[0].Value = model.rssi;
            parameters[1].Value = model.mobile_id;
            parameters[2].Value = model.mac;
            parameters[3].Value = model.coord_id;

            object obj = DbHelperSQL.ExecuteSql(strSql.ToString(),parameters); 
            model.id = Convert.ToInt32(obj);
            if (model.id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Models.Rssi model)
        {
         
            return true;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_wifi_rssi ");
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
        public Models.Rssi GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,rssi,mobile_id,mac,coord_id");
            strSql.Append(" from tb_wifi_rssi");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Models.Rssi model = new Models.Rssi();
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

        public DataSet GetRssiList(string whereStr)
        {
            var sql = "select rssi,mac,coord_id from tb_wifi_rssi";
            if (whereStr.Length > 0)
            {
                sql += " where " + whereStr;
            }
            DataSet ds = DbHelperSQL.Query(sql);
            return ds;
        }
        #endregion



        #region 私有方法=================================
        ///<summary>
        /// 将对象转换为实体
        /// </summary>
        private Models.Rssi DataRowToModel(DataRow row)
        {
            Models.Rssi model = new Models.Rssi();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["coord_id"] != null && row["coord_id"].ToString() != "")
                {
                    model.coord_id = Int32.Parse(row["coord_id"].ToString());
                }
                if (row["rssi"] != null && row["rssi"].ToString() != "")
                {
                    model.rssi = Int32.Parse (row["rssi"].ToString());
                }
                if (row["mobile_id"] != null && row["mobile_id"].ToString() != "")
                {
                    model.mobile_id = Int32.Parse(row["mobile_id"].ToString());
                }
                if (row["mac"] != null && row["mac"].ToString() != "")
                {
                    model.mac = row["mac"].ToString();
                }
            }
            return model;
        }
        #endregion


    }
}