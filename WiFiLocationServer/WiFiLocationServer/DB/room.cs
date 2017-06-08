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
    public class room
    {
        #region 公用方法=================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_room");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Models.Room model)
        {
            return 0;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Models.Room model)
        {
         
            return true;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_room ");
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
        public Models.Room GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" from tb_room");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Models.Room model = new Models.Room();
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

        public DataSet GetRoomDetial(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" from tb_room");
            strSql.Append(" where id="+id);
            strSql.Append(";select id,actual_coord,location_coord from tb_location_log where room_id="+ id + " and location_time > " + (int.Parse(GetTimeStamp()) - 300));
            return  DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetRoomList()
        {
            string sql = "select id,room_name from tb_room";
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
        private Models.Room DataRowToModel(DataRow row)
        {
            Models.Room model = new Models.Room();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.id = int.Parse(row["id"].ToString());
                }
                if (row["room_name"] != null && row["room_name"].ToString() != "")
                {
                    model.room_name = row["room_name"].ToString();
                }
                if (row["floor_num"] != null && row["floor_num"].ToString() != "")
                {
                    model.floor_num = row["floor_num"].ToString();
                }
                if (row["left_up"] != null && row["left_up"].ToString() != "")
                {
                    model.left_up = row["left_up"].ToString();
                }
                if (row["right_up"] != null && row["right_up"].ToString() != "")
                {
                    model.right_up = row["right_up"].ToString();
                }
                if (row["left_down"] != null && row["left_down"].ToString() != "")
                {
                    model.left_down = row["left_down"].ToString();
                }
                if(row["right_down"] != null && row["right_down"].ToString() != "")
                {
                    model.right_down = row["right_down"].ToString();
                }
            }
            return model;
        }

        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        } 
        #endregion


    }
}