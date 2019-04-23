using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Express.Common.TableCache
{
    public class DataTableCache
    {
        /// <summary>
        /// 存储table
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">表</param>
        /// <returns></returns>
        public static bool Store(string key,DataTable value)
        {
            if (CreateDBIfNotExist())
            {
                string sql = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + key + "'";
                if (DbHelperSQLite.Exists(sql))
                {
                    Drop(key);
                    CreateTable(key, value);
                    InsertData(key, value);
                }
                else
                {
                    CreateTable(key ,value);
                    InsertData(key, value);
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取table
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static DataTable Get(string key)
        {
            if (CreateDBIfNotExist())
            {
                string sql = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + key + "'";
                if (DbHelperSQLite.Exists(sql))
                {
                    sql = "select * from " + key;
                    return DbHelperSQLite.Query(sql).Tables[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取table
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="sqlWhere">条件</param>
        /// <returns></returns>
        public static DataTable Get(string key,string sqlWhere)
        {
            if (CreateDBIfNotExist())
            {
                string sql = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + key + "'";
                if (DbHelperSQLite.Exists(sql))
                {
                    sql = "select * from " + key;
                    if (!string.IsNullOrEmpty(sqlWhere))
                        sql += " where " + sqlWhere;
                    return DbHelperSQLite.Query(sql).Tables[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static bool Exist(string key)
        {
            if (CreateDBIfNotExist())
            {
                string sql = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + key + "'";
                if (DbHelperSQLite.Exists(sql))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除table
        /// </summary>
        /// <param name="key">表名</param>
        public static void Drop(string key)
        {
            string sql = "DROP TABLE IF EXISTS " + key ;
            DbHelperSQLite.ExecuteSql(sql);
        }
        /// <summary>
        /// 按照开头字母删除table
        /// </summary>
        /// <param name="key"></param>
        public static void DropBySearch(string key)
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' and name like '"+key+"%' ORDER BY name";
            DataTable dt = DbHelperSQLite.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                ArrayList sqlArray = new ArrayList();
                foreach (DataRow dr in dt.Rows)
                {
                    sqlArray.Add("DROP TABLE IF EXISTS " + dr[0].ToString());
                }
                DbHelperSQLite.ExecuteSqlTran(sqlArray);
            }
        }
        /// <summary>
        /// 建表
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="value">表</param>
        private static void CreateTable(string key, DataTable value)
        {
            StringBuilder sb = new StringBuilder();
            foreach(DataColumn dc in value.Columns)
            {
                sb.Append(dc.ColumnName + " TEXT,");
            }
            string sql = "CREATE TABLE IF NOT EXISTS " + key + "(" + sb.ToString().Trim(',') + ")";
            DbHelperSQLite.ExecuteSql(sql);

        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="key">表名</param>
        /// <param name="value">表</param>
        private static void InsertData(string key, DataTable value)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < value.Columns.Count; i++)
            {
                sb.Append(value.Columns[i].ColumnName + ",");
            }
            ArrayList sqlArray = new ArrayList();
            foreach(DataRow dr in value.Rows)
            {
                StringBuilder sbs = new StringBuilder();
                for (int j = 0; j < value.Columns.Count; j++)
                {
                    sbs.Append("'" + dr[value.Columns[j].ColumnName].ToString() + "',");
                }
                sqlArray.Add("insert into " + key + "(" + sb.ToString().Trim(',') + ") values(" + sbs.ToString().Trim(',') + ")");
            }

            DbHelperSQLite.ExecuteSqlTran(sqlArray);

        }
        /// <summary>
        /// 判断存储库是否存在
        /// </summary>
        /// <returns></returns>
        public static bool IsDBExist()
        {
            string path = HttpRuntime.AppDomainAppPath.ToString().TrimEnd('\\') + "\\appCache.db";
            if (File.Exists(path))
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 如果存储库不存在则自动创建
        /// </summary>
        /// <returns></returns>
        private static bool CreateDBIfNotExist()
        {
            string path = HttpRuntime.AppDomainAppPath.ToString().TrimEnd('\\') + "\\appCache.db";
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                try
                {
                    SQLiteConnection.CreateFile(path);
                    SQLiteConnection cnn = new SQLiteConnection("Data Source = "+ path);
                    cnn.Open();
                    cnn.ChangePassword("Waes5256");
                    cnn.Close();
                    cnn.Dispose();
                    return true;
                }
                catch  {
                    return false;
                }
            }

        }
    }
    public class DbHelperSQLite
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.
        public static string connectionString = "Data Source=" + HttpRuntime.AppDomainAppPath.ToString().TrimEnd('\\') + "\\appCache.db;Password=Waes5256";
        public DbHelperSQLite()
        {
        }


        #region 公用方法

        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Exists(string strSql, params SQLiteParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(SQLString, connection);
                SQLiteParameter myParameter = new SQLiteParameter("@content", DbType.String);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(strSQL, connection);
                SQLiteParameter myParameter = new SQLiteParameter("@fs", DbType.Binary);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.SQLite.SQLiteException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public static SQLiteDataReader ExecuteReader(string strSQL)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand(strSQL, connection);
            try
            {
                connection.Open();
                SQLiteDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }


        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SQLite.SQLiteException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }




        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SQLiteDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SQLiteDataReader</returns>
        public static SQLiteDataReader ExecuteReader(string SQLString, params SQLiteParameter[] cmdParms)
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SQLiteDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params SQLiteParameter[] cmdParms)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SQLiteDataAdapter da = new SQLiteDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SQLite.SQLiteException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(SQLiteCommand cmd, SQLiteConnection conn, SQLiteTransaction trans, string cmdText, SQLiteParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion



    }
}
