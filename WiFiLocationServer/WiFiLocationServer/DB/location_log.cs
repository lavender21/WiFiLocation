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
    public class location_log
    {
        #region 公用方法=================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_location_log");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Models.location_log model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_location_log(actual_coord,location_coord,room_id,mobile_id,location_algorithm)");
            strSql.Append(" values (@actual_coord,@location_coord,@room_id,@mobile_id,@location_algorithm)");
            SqlParameter[] parameters = {
                    new SqlParameter("@actual_coord", SqlDbType.VarChar,10),
                    new SqlParameter("@location_coord", SqlDbType.VarChar, 10),
                    new SqlParameter("@room_id",SqlDbType.Int),
                    new SqlParameter("@mobile_id",SqlDbType.Int),
                    new SqlParameter("location_algorithm", SqlDbType.Int)
                                        };
            parameters[0].Value = model.actual_coord;
            parameters[1].Value = model.location_coord;
            parameters[2].Value = model.room_id;
            parameters[3].Value = model.mobile_id;
            parameters[4].Value = model.location_algorithm;

            object obj = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            model.id = Convert.ToInt32(obj);

            return model.id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Models.location_log model)
        {
         
            return true;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_location_log ");
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
        public Models.location_log GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,model_name");
            strSql.Append(" from tb_location_log");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Models.location_log model = new Models.location_log();
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

        public DataSet GetlocationLogList()
        {
            string sql = "select id,location_log_name from tb_location_log";
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



        #region 私有方法=================================
        ///<summary>
        /// 将对象转换为实体
        /// </summary>
        private Models.location_log DataRowToModel(DataRow row)
        {
            Models.location_log model = new Models.location_log();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
             
            }
            return model;
        }
        #endregion


    }
}