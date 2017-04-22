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
    public class fingerprint
    {
        #region 公用方法=================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_wifi_fingerprint");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Models.Fingerprint model)
        {           
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_wifi_fingerprint(coord,memory,flag,addtime)");
            strSql.Append(" values (@coord,@memory,@flag,@addtime);select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@coord", SqlDbType.VarChar,10),
                    new SqlParameter("@memory", SqlDbType.VarChar),
                    new SqlParameter("@flag", SqlDbType.Int),
                    new SqlParameter("@addtime", SqlDbType.VarChar,50)};
            parameters[0].Value = model.coord;
            parameters[1].Value = model.memory;
            parameters[2].Value = model.flag;
            parameters[3].Value = model.addtime;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters); 
            model.id = Convert.ToInt32(obj);

            return model.id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Models.Fingerprint model)
        {
         
            return true;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_wifi_fingerprint ");
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
        public Models.Fingerprint GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,coord,memory,flag,addtime");
            strSql.Append(" from tb_wifi_fingerprint");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Models.Fingerprint model = new Models.Fingerprint();
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
        
        public DataSet getCoordList (string idlist)
        {
            var sql = "select coord from tb_wifi_fingerprint";
            if (idlist.Length > 0)
            {
                sql += " where id in(" + idlist + ")";
            }
            DataSet ds =  DbHelperSQL.Query(sql);
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




        #region 私有方法=================================
        ///<summary>
        /// 将对象转换为实体
        /// </summary>
        private Models.Fingerprint DataRowToModel(DataRow row)
        {
            Models.Fingerprint model = new Models.Fingerprint();
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
                if (row["flag"] != null && row["flag"].ToString() != "")
                {
                    model.flag = Int32.Parse(row["flag"].ToString());
                }
                if (row["memory"] != null && row["memory"].ToString() != "")
                {
                    model.memory = row["memory"].ToString();
                }
                if (row["addtime"] != null && row["addtime"].ToString() != "")
                {
                    model.addtime = row["addtime"].ToString();
                }
            }
            return model;
        }
        #endregion


    }
}