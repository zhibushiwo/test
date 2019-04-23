using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WAES.DBUtility;
using System.Data.SqlClient;
using System.Reflection;

namespace WAES.DAL
{
    /// <summary>
    /// DAL基础方法
    /// </summary>
    /// <typeparam name="T">Model类型</typeparam>
    public class DALBaseSqlServer<T>
    {
        public StringBuilder property = new StringBuilder();
        public StringBuilder propertyWithoutPrimaryKey = new StringBuilder();
        public StringBuilder propertyWithAt = new StringBuilder();
        public List<string> listProperty = new List<string>();
        public List<string> listPropertyWithAt = new List<string>();
        public List<object> listPropertyValue = new List<object>();
        string tableName = "";string primaryKey = "";
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="model">Model实例</param>
        /// <param name="_tableName">表名</param>
        /// <param name="_primaryKey">主键（仅支持单一主键）</param>
        public DALBaseSqlServer(T model,string _tableName,string _primaryKey)
        {
            //得到类型
            Type type = typeof(T);
            //取得属性集合
            PropertyInfo[] pi = type.GetProperties();
            foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
            {
                property.Append(p.Name + ",");
                if (p.Name != _primaryKey)
                    propertyWithoutPrimaryKey.Append(p.Name + ",");
                propertyWithAt.Append("@" + p.Name + ",");
                listProperty.Add(p.Name);
                listPropertyWithAt.Add("@"+p.Name);
                listPropertyValue.Add(p.GetValue(model));
                tableName = _tableName;
                primaryKey = _primaryKey;
            }
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="_tableName"></param>
        /// <param name="_primaryKey"></param>
        public DALBaseSqlServer(string _tableName, string _primaryKey)
        {
            //得到类型
            Type type = typeof(T);
            //取得属性集合
            PropertyInfo[] pi = type.GetProperties();
            foreach (System.Reflection.PropertyInfo p in pi)
            {
                property.Append(p.Name + ",");
                if (p.Name != _primaryKey)
                    propertyWithoutPrimaryKey.Append(p.Name + ",");
                propertyWithAt.Append("@" + p.Name + ",");
                listProperty.Add(p.Name);
                listPropertyWithAt.Add("@" + p.Name);
                tableName = _tableName;
                primaryKey = _primaryKey;
            }
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public bool Exists(object keyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ["+tableName+"]");
            strSql.Append(" where ");
            strSql.Append(primaryKey + "= @"+ primaryKey);
            SqlParameter[] parameters = {
                    new SqlParameter("@"+primaryKey, keyValue)
            };

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="colName">列名</param>
        /// <param name="keyValue">值</param>
        /// <returns></returns>
        public bool Exists(string colName, object keyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [" + tableName+"]");
            strSql.Append(" where ");
            strSql.Append(colName + "= @" + colName);
            SqlParameter[] parameters = {
                    new SqlParameter("@"+colName, keyValue)
            };

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        private void UpdateModel(T model)
        {
            listPropertyValue.Clear();
            foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
            {
                listPropertyValue.Add(p.GetValue(model));
            }
        }
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(T model)
        {
            UpdateModel(model);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [" + tableName + "]([");
            strSql.Append(propertyWithoutPrimaryKey.ToString().Trim(',').Replace(",", "],["));
            strSql.Append("]) values (");
            strSql.Append(propertyWithAt.ToString().Trim(',').Replace("@" + primaryKey + ",", ""));
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            List<SqlParameter> listPara = new List<SqlParameter>();
            for (int i = 0; i < listProperty.Count; i++)
            {
                if (listProperty[i] != primaryKey)
                    listPara.Add(new SqlParameter(listPropertyWithAt[i], listPropertyValue[i]));
            }
            SqlParameter[] parameters = listPara.ToArray();

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(T model)
        {
            UpdateModel(model);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ["+tableName+"] set ");
            for (int i = 0; i < listProperty.Count; i++)
            {
                if (listProperty[i] == primaryKey)
                    continue;
                if (i == listProperty.Count - 1)
                    strSql.Append(" [" + listProperty[i] + "] = " + listPropertyWithAt[i] + " ");
                else
                    strSql.Append(" [" + listProperty[i] + "] = " + listPropertyWithAt[i] + " , ");
            }
            strSql.Append(" where " + primaryKey + "=@" + primaryKey + " ");
            List<SqlParameter> listPara = new List<SqlParameter>();
            for (int i = 0; i < listProperty.Count; i++)
            {
                listPara.Add(new SqlParameter(listPropertyWithAt[i], listPropertyValue[i]));
            }
            SqlParameter[] parameters = listPara.ToArray();

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public bool Delete(object keyValue)
        {
            int rows = 0;
            if (listProperty.IndexOf("Active_Flag") > -1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [" + tableName + "] ");
                strSql.Append(" set Active_Flag=0 ");
                strSql.Append(" where " + primaryKey + "=@" + primaryKey + "");
                SqlParameter[] parameters = {
                    new SqlParameter("@"+primaryKey, keyValue)
                };
                rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            else if (listProperty.IndexOf("Status") > -1)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update [" + tableName + "] ");
                strSql.Append(" set status=0 ");
                strSql.Append(" where " + primaryKey + "=@" + primaryKey + "");
                SqlParameter[] parameters = {
                    new SqlParameter("@"+primaryKey, keyValue)
                };
                rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from [" + tableName + "] ");
                strSql.Append(" where " + primaryKey + "=@" + primaryKey + "");
                SqlParameter[] parameters = {
                    new SqlParameter("@"+primaryKey, keyValue)
                };
                rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据条件条件删除
        /// </summary>
        /// <param name="sqlwhere">条件语句（不需要写where）</param>
        /// <returns></returns>
        public bool DeleteWhere(string sqlwhere)
        {
            if (string.IsNullOrEmpty(sqlwhere))
                return false;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [" + tableName + "] ");
            strSql.Append(" where " + sqlwhere);

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="Idlist">主键值</param>
        /// <returns></returns>
        public bool DeleteList(string Idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ["+tableName+"] ");
            strSql.Append(" where "+primaryKey+" in (" + Idlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据SQL语句获取第一个实例
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public T GetModelTop1(string sqlWhere)
        {
            DataSet ds = GetList(sqlWhere);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                return DataRowToModel(ds.Tables[0].Rows[0]);
            else
                return default(T);
        }
        /// <summary>
        /// 获取model列表
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public List<T> GetModelListBySqlwhere(string sqlWhere)
        {
            DataSet ds = GetList(sqlWhere);
            List<T> models = new List<T>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    models.Add(DataRowToModel(dr));
                return models;
            }
            else
                return null;
        }
        /// <summary>
        /// 获取model列表
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="strWhere"></param>
        /// <param name="filedOrder"></param>
        /// <returns></returns>
        public List<T> GetModelListBySqlwhere(int Top, string strWhere, string filedOrder)
        {
            DataSet ds = GetList(Top, strWhere, filedOrder);
            List<T> models = new List<T>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    models.Add(DataRowToModel(dr));
                return models;
            }
            else
                return null;
        }
        /// <summary>
        /// 分页获取model List
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public List<T> GetModelListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            DataSet ds = GetListByPage(strWhere, orderby, startIndex, endIndex);
            List<T> models = new List<T>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                    models.Add(DataRowToModel(dr));
                return models;
            }
            else
                return null;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T DataRowToModel(DataRow row)
        {
            T model = default(T);
            if (row != null)
            {
                if (model == null)
                    model = System.Activator.CreateInstance<T>();
                //得到类型
                Type type = typeof(T);
                //取得属性集合
                PropertyInfo[] pi = type.GetProperties();
                foreach (PropertyInfo item in pi)
                {
                    //给属性赋值
                    if (row[item.Name] != null && row[item.Name] != DBNull.Value)
                    {
                        if (item.PropertyType == typeof(System.Nullable<System.DateTime>))
                        {
                            item.SetValue(model, Convert.ToDateTime(row[item.Name].ToString()), null);
                        }
                        else if (item.PropertyType == typeof(System.Nullable<System.Int32>))
                        {
                            item.SetValue(model, Convert.ToInt32(row[item.Name].ToString()), null);
                        }
                        else if (item.PropertyType == typeof(System.Nullable<System.Boolean>))
                        {
                            item.SetValue(model, Convert.ToBoolean(row[item.Name].ToString()), null);
                        }
                        else
                        {
                            item.SetValue(model, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                        }
                    }
                }
                return model;
            }
            return model;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ["+property.ToString().Trim(',').Replace(",","],["));
            strSql.Append("] FROM ["+tableName+"] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }



        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" [" + property.ToString().Trim(',').Replace(",", "],[") + "] ");
            strSql.Append(" FROM [" + tableName + "] ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 获取记录数量
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ["+tableName+"]");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T." + primaryKey + " desc");
            }
            strSql.Append(")AS Row, T.*  from ["+tableName+"] T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetViewListByPage(string viewName, string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T." + primaryKey + " desc");
            }
            strSql.Append(")AS Row, T.*  from [" + viewName + "] T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }

        public int GetViewRecordCount(string viewName, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM [" + viewName + "]");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 获取视图列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetViewList(string viewName, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM  ");
            strSql.Append(""+ viewName + "");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
        public int ImportByTran(List<T> models)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ["+tableName+"]([");
            strSql.Append(propertyWithoutPrimaryKey.ToString().Trim(',').Replace(",", "],["));
            strSql.Append("]) values (");
            strSql.Append(propertyWithAt.ToString().Trim(',').Replace("@"+primaryKey+",", ""));
            strSql.Append(") ");
            List<CommandInfo> listCmd = new List<CommandInfo>();
            foreach (T model in models)
            {
                UpdateModel(model);
                List<SqlParameter> listPara = new List<SqlParameter>();
                for (int i = 0; i < listProperty.Count; i++)
                {
                    if (listProperty[i] != primaryKey)
                        listPara.Add(new SqlParameter(listPropertyWithAt[i], listPropertyValue[i]));
                }
                SqlParameter[] parameters = listPara.ToArray();
                listCmd.Add(new CommandInfo(strSql.ToString(), parameters));
            }
            return DbHelperSQL.ExecuteSqlTran(listCmd);
        }
        public int UpdataList(List<T> models)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [" + tableName + "] set ");
            for (int i = 0; i < listProperty.Count; i++)
            {
                if (listProperty[i] == primaryKey)
                    continue;
                if (i == listProperty.Count - 1)
                    strSql.Append(" [" + listProperty[i] + "] = " + listPropertyWithAt[i] + " ");
                else
                    strSql.Append(" [" + listProperty[i] + "] = " + listPropertyWithAt[i] + " , ");
            }
            strSql.Append(" where " + primaryKey + "=@" + primaryKey + " ");

            List<CommandInfo> listCmd = new List<CommandInfo>();
            foreach (T model in models)
            {
                UpdateModel(model);
                List<SqlParameter> listPara = new List<SqlParameter>();
                for (int i = 0; i < listProperty.Count; i++)
                {
                    //if (listProperty[i] != primaryKey)
                        listPara.Add(new SqlParameter(listPropertyWithAt[i], listPropertyValue[i]));
                }
                SqlParameter[] parameters = listPara.ToArray();
                listCmd.Add(new CommandInfo(strSql.ToString(), parameters));
            }
            return DbHelperSQL.ExecuteSqlTran(listCmd);
        }
        //public int UpdateByTran(List<T> models)
        //{
        //    StringBuilder strSql = new StringBuilder();
        //    strSql.Append("insert into [" + tableName + "]([");
        //    strSql.Append(propertyWithoutPrimaryKey.ToString().Trim(',').Replace(",", "],["));
        //    strSql.Append("]) values (");
        //    strSql.Append(propertyWithAt.ToString().Trim(',').Replace("@" + primaryKey + ",", ""));
        //    strSql.Append(") ");
        //    List<CommandInfo> listCmd = new List<CommandInfo>();
        //    foreach (T model in models)
        //    {
        //        UpdateModel(model);
        //        List<SqlParameter> listPara = new List<SqlParameter>();
        //        for (int i = 0; i < listProperty.Count; i++)
        //        {
        //            if (listProperty[i] != primaryKey)
        //                listPara.Add(new SqlParameter(listPropertyWithAt[i], listPropertyValue[i]));
        //        }
        //        SqlParameter[] parameters = listPara.ToArray();
        //        listCmd.Add(new CommandInfo(strSql.ToString(), parameters));
        //    }
        //    return DbHelperSQL.ExecuteSqlTran(listCmd);
        //}
    }
}
