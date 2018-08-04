using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace PG.SQLite
{
    public class SQLiteDate
    {

        private SqliteConnection dbConnection;

        private SqliteCommand dbCommand;

        private SqliteDataReader reader;

        public SQLiteDate(string connectionString)
        {
            OpenDB(connectionString);
            CloseSqlConnection();
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="connectionString"></param>
        public void OpenDB(string connectionString)
        {
            try
            {
                dbConnection = new SqliteConnection(connectionString);

                dbConnection.Open();

                //Debug.Log("Connected to db");
            }
            catch (Exception e)
            {
                string temp1 = e.ToString();
                Debug.Log(temp1);
            }

        }
        private void OpenSql()
        {
            try
            {
                dbConnection.Open();
                //Debug.Log("Connected to db");
            }
            catch (Exception e)
            {
                string temp1 = e.ToString();
                Debug.Log(temp1);
            }
        }
        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        private SqliteDataReader ExecuteQuery(string sqlQuery)
        {
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = sqlQuery;
            reader = dbCommand.ExecuteReader();
            return reader;
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        private void CloseSqlConnection()
        {
            if (dbCommand != null)
            {
                dbCommand.Dispose();
            }
            dbCommand = null;
            if (reader != null)
            {
                reader.Dispose();
            }
            reader = null;
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
            //Debug.Log("Disconnected from db.");
        }
        /// <summary>
        /// 执行数据库语句
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="fn"></param>
        public void ExecuteQuery(string sqlQuery, Action<SqliteDataReader> fn = null)
        {
            OpenSql();
            try
            {
                SqliteDataReader tempreader = ExecuteQuery(sqlQuery);
                if (fn != null)
                    fn(tempreader);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="column">列名（标签）</param>
        /// <param name="value">值（与列名对应）</param>
        public void InsertInfo(string TableName, string[] column, string[] value)
        {
            if (string.IsNullOrEmpty(TableName))
                throw new Exception("TableName is null");
            if (column.Length != value.Length)
                throw new Exception("column and value count is not equal");
            OpenSql();
            string insertsql = "INSERT INTO {0} ({1}) VALUES ({2})";
            string keystr = "";
            string valuestr = "";
            for (int i = 0; i < column.Length; i++)
            {
                if (i == 0)
                    keystr = column[i];
                else
                    keystr += "," + column[i];
            }
            for (int i = 0; i < value.Length; i++)
            {
                if (i == 0)
                    valuestr = value[i];
                else
                    valuestr += "," + value[i];
            }
            string sqlstr = string.Format(insertsql, TableName, keystr, valuestr);
            try
            {
                ExecuteQuery(sqlstr);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("insert sql error: " + ex.Message);
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="value">内容</param>
        public void InsertInfo(string TableName, string value)
        {
            if (string.IsNullOrEmpty(TableName))
                throw new Exception("TableName is null");
            OpenSql();
            string insertsql = "INSERT INTO {0} {1}";
            string sqlstr = string.Format(insertsql, TableName, value);
            try
            {
                ExecuteQuery(sqlstr);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("insert sql error: " + ex.Message);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="column">列名（标签）</param>
        /// <param name="value">值（与列名对应）</param>
        /// <param name="where">数据索引</param>
        public void UpdataInfo(string TableName, string[] column, string[] value, string where)
        {
            if (string.IsNullOrEmpty(TableName))
                throw new Exception("TableName is null");
            if (column.Length != value.Length)
                throw new Exception("column and value count is not equal");
            if (string.IsNullOrEmpty(where))
                throw new Exception("where is null");
            OpenSql();
            string updatasql = "UPDATE {0} SET {1} WHERE {2}";
            string keystr = "";
            for (int i = 0; i < column.Length; i++)
            {
                if (i == 0)
                    keystr = column[i] + "=" + value[i];
                else
                    keystr += "," + column[i] + "=" + value[i];
            }
            string sqlstr = string.Format(updatasql, TableName, keystr, where); try
            {
                ExecuteQuery(sqlstr);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("updata sql error: " + ex.Message);
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="where"></param>
        public void DeleteInfo(string TableName, string where)
        {
            if (string.IsNullOrEmpty(TableName))
                throw new Exception("TableName is null");
            if (string.IsNullOrEmpty(where))
                throw new Exception("where is null");
            OpenSql();
            string sqlstr = string.Format("DELETE FROM {0} WHERE {1}", TableName, where);
            try
            {
                ExecuteQuery(sqlstr);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("Delete sql error: " + ex.Message);
            }
        }
        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="fn">查询回调</param>
        /// <param name="column">查询内容（可为空）</param>
        /// <param name="where">查询条件（可为空）</param>
        public void SelectInfo(string TableName, Action<SqliteDataReader> fn = null, string[] column = null, string where = null)
        {
            if (string.IsNullOrEmpty(TableName))
                throw new Exception("TableName is null");
            OpenSql();
            string sqlstr = "SELECT {0} FROM {1}";
            if (column == null)
                sqlstr = string.Format("SELECT * FROM {0}", TableName);
            else if (column.Length == 0)
                sqlstr = string.Format("SELECT * FROM {0}", TableName);
            else
            {
                string keystr = "";
                for (int i = 0; i < column.Length; i++)
                {
                    if (i == 0)
                        keystr = column[i];
                    else
                        keystr += "," + column[i];
                }
                sqlstr = string.Format("SELECT {0} FROM {1}", keystr, TableName);
            }
            if (where != null)
            {
                sqlstr += " WHERE " + where;
            }
            try
            {
                SqliteDataReader data = ExecuteQuery(sqlstr);
                if (fn != null)
                    fn(data);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("Delete sql error: " + ex.Message);
            }
        }

        public void SelectInfo(string TableName, Action<SqliteDataReader> fn = null, string column = null, string where = null)
        {
            if (string.IsNullOrEmpty(TableName))
                throw new Exception("TableName is null");
            OpenSql();
            string sqlstr = "SELECT {0} FROM {1}";
            if (column == null)
                sqlstr = string.Format("SELECT * FROM {0}", TableName);
            else if (column.Length == 0)
                sqlstr = string.Format("SELECT * FROM {0}", TableName);
            else
            {
                sqlstr = string.Format("SELECT {0} FROM {1}", column, TableName);
            }
            if (where != null)
            {
                sqlstr += " WHERE " + where;
            }
            try
            {
                SqliteDataReader data = ExecuteQuery(sqlstr);
                if (fn != null)
                    fn(data);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("Delete sql error: " + ex.Message);
            }
        }

        /// <summary>Sele
        /// 创建一张表
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="column">列名</param>
        /// <param name="columnType">列的类型</param>
        public void CreateTable(string TableName, string[] column, string[] columnType)
        {
            if (column.Length != columnType.Length)
            {
                throw new Exception("column and value count is not equal");
            }
            OpenSql();
            string keystr = "";
            for (int i = 0; i < column.Length; i++)
            {
                if (i == 0)
                    keystr = column[i] + " " + columnType[i];
                else
                    keystr += "," + column[i] + " " + columnType[i];
            }
            string sqlstr = string.Format("CREATE TABLE {0} ({1})", TableName, keystr);
            try
            {
                SqliteDataReader data = ExecuteQuery(sqlstr);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("Delete sql error: " + ex.Message);
            }
        }
        /// <summary>
        /// 创建一张表
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="TableInfo">表的结构语句</param>
        public void CreateTable(string TableName, string TableInfo)
        {
            OpenSql();
            string sqlstr = string.Format("CREATE TABLE {0} ({1})", TableName, TableInfo);
            try
            {
                SqliteDataReader data = ExecuteQuery(sqlstr);
                CloseSqlConnection();
            }
            catch (Exception ex)
            {
                CloseSqlConnection();
                throw new Exception("Delete sql error: " + ex.Message);
            }
        }
        /// <summary>
        /// 判断某张表是否存在
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public bool ExistsTable(string TableName)
        {
            try
            {
                OpenSql();
                ExecuteQuery("Select * from " + TableName);
                CloseSqlConnection();
                return true;
            }
            catch
            {
                CloseSqlConnection();
                return false;
            }
        }
    }
}