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
    public class DALBaseSqlServerAdv<T>
    {
        StringBuilder property_table = new StringBuilder();
        StringBuilder propertyWithoutPrimaryKey_table = new StringBuilder();
        StringBuilder propertyWithAt_table = new StringBuilder();
        StringBuilder property_view = new StringBuilder();
        StringBuilder propertyWithoutPrimaryKey_view = new StringBuilder();
        StringBuilder propertyWithAt_view = new StringBuilder();
        List<string> listProperty_table = new List<string>();
        List<string> listPropertyWithAt_table = new List<string>();
        List<object> listPropertyValue_table = new List<object>();
        List<string> listProperty_view = new List<string>();
        List<string> listPropertyWithAt_view = new List<string>();
        List<object> listPropertyValue_view = new List<object>();
        string tableName = ""; string viewName = ""; string primaryKey = "";
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="model">Model实例</param>
        /// <param name="_tableName">表名</param>
        /// <param name="_primaryKey">主键（仅支持单一主键）</param>
        public DALBaseSqlServerAdv(T model, string _tableName, string _viewName, string _primaryKey)
        {

            DataTable dt = DbHelperSQL.Query("select column_name from information_schema.columns where table_name='" + _tableName + "'").Tables[0];
            List<string> lstTable_Columns = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                lstTable_Columns.Add(dr["Column_Name"].ToString());
            }
            foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
            {
                if (lstTable_Columns.Exists(c => c.ToString() == p.Name))
                {
                    property_table.Append(p.Name + ",");
                    if (p.Name != _primaryKey)
                        propertyWithoutPrimaryKey_table.Append(p.Name + ",");
                    propertyWithAt_table.Append("@" + p.Name + ",");
                    listProperty_table.Add(p.Name);
                    listPropertyWithAt_table.Add("@" + p.Name);
                    listPropertyValue_table.Add(p.GetValue(model));
                }
                property_view.Append(p.Name + ",");
                if (p.Name != _primaryKey)
                    propertyWithoutPrimaryKey_view.Append(p.Name + ",");
                propertyWithAt_view.Append("@" + p.Name + ",");
                listProperty_view.Add(p.Name);
                listPropertyWithAt_view.Add("@" + p.Name);
                listPropertyValue_view.Add(p.GetValue(model));

                viewName = _viewName;
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
            strSql.Append("select count(1) from [" + tableName + "]");
            strSql.Append(" where ");
            strSql.Append(primaryKey + "= @" + primaryKey);
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
            strSql.Append("select count(1) from [" + tableName + "]");
            strSql.Append(" where ");
            strSql.Append(colName + "= @" + colName);
            SqlParameter[] parameters = {
                    new SqlParameter("@"+colName, keyValue)
            };

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        private void UpdateModel(T model)
        {
            DataTable dt = DbHelperSQL.Query("select column_name from information_schema.columns where table_name='" + tableName + "'").Tables[0];
            List<string> lstTable_Columns = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                lstTable_Columns.Add(dr["Column_Name"].ToString());
            }
            listPropertyValue_table.Clear();
            listPropertyValue_view.Clear();

            foreach (System.Reflection.PropertyInfo p in model.GetType().GetProperties())
            {

                if (lstTable_Columns.Exists(c => c.ToString() == p.Name))
                {
                    listPropertyValue_table.Add(p.GetValue(model));
                }
                listPropertyValue_view.Add(p.GetValue(model));
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
            strSql.Append(propertyWithoutPrimaryKey_table.ToString().Trim(',').Replace(",", "],["));
            strSql.Append("]) values (");
            strSql.Append(propertyWithAt_table.ToString().Trim(',').Replace("@" + primaryKey + ",", ""));
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY");
            List<SqlParameter> listPara = new List<SqlParameter>();
            for (int i = 0; i < listProperty_table.Count; i++)
            {
                if (listProperty_table[i] != primaryKey)
                    listPara.Add(new SqlParameter(listPropertyWithAt_table[i], listPropertyValue_table[i]));
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
            strSql.Append("update [" + tableName + "] set ");
            for (int i = 0; i < listProperty_table.Count; i++)
            {
                if (listProperty_table[i] == primaryKey)
                    continue;
                if (i == listProperty_table.Count - 1)
                    strSql.Append(" [" + listProperty_table[i] + "] = " + listPropertyWithAt_table[i] + " ");
                else
                    strSql.Append(" [" + listProperty_table[i] + "] = " + listPropertyWithAt_table[i] + " , ");
            }
            strSql.Append(" where " + primaryKey + "=@" + primaryKey + " ");
            List<SqlParameter> listPara = new List<SqlParameter>();
            for (int i = 0; i < listProperty_table.Count; i++)
            {
                listPara.Add(new SqlParameter(listPropertyWithAt_table[i], listPropertyValue_table[i]));
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [" + tableName + "] ");
            strSql.Append(" where " + primaryKey + "=@" + primaryKey + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@"+primaryKey, keyValue)
            };

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
            strSql.Append("delete from [" + tableName + "] ");
            strSql.Append(" where " + primaryKey + " in (" + Idlist + ")  ");
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
            strSql.Append("select [" + property_view.ToString().Trim(',').Replace(",", "],["));
            strSql.Append("] FROM [" + viewName + "] ");
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
            strSql.Append(" [" + property_view.ToString().Trim(',').Replace(",", "],[") + "] ");
            strSql.Append(" FROM [" + viewName + "] ");
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
            strSql.Append("select count(1) FROM [" + tableName + "]");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.ExecuteSql(strSql.ToString());
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
            strSql.Append(")AS Row, T.*  from [" + viewName + "] T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperSQL.Query(strSql.ToString());
        }
    }
}
