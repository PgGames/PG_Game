using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.SQLite;
using System.IO;
using PG.Manager;


namespace PG.Manager
{
    public class SQLManager : Manager<SQLManager>
    {
        private SQLiteDate m_SQLData;
        //数据库地址
        private string m_SQL_Path;
        //数据库名称
        private string m_SQL_Name = "PGGames.db";

        #region 固定信息库

        //固定信息数据库
        private SQLiteDate m_Const_SQLData;
        //固定信息数据库地址
        private string m_Const_SQL_Path;
        //固定信息数据库名称
        private string m_Const_SQL_Name = "/Const.db";

        #endregion

        public void Init()
        {
            m_SQL_Path = Application.persistentDataPath + "/Games/";
            if (!Directory.Exists(m_SQL_Path))
                Directory.CreateDirectory(m_SQL_Path);
            m_SQLData = new SQLiteDate("data source=" + m_SQL_Path + m_SQL_Name);

            CreateTable();
            m_Const_SQL_Path = Application.streamingAssetsPath;
            if (!Directory.Exists(m_Const_SQL_Path))
                Directory.CreateDirectory(m_Const_SQL_Path);
            m_Const_SQLData = new SQLiteDate("data source=" + m_Const_SQL_Path + m_Const_SQL_Name);
        }


        #region 外用数据库

        #endregion


        #region 创建表格

        /// <summary>
        /// 创建表格
        /// </summary>
        private void CreateTable()
        {
            CreateUserTable();
        }

        /// <summary>
        /// 创建用户信息表
        /// </summary>
        protected void CreateUserTable()
        {
            if (!m_SQLData.ExistsTable("UserInfo"))
            {
                if (!m_SQLData.ExistsTable("UserInfo"))
                {
                    //创建用户信息表
                    //表的结构
                    /*
                     * 表的字段      |  字段含义          字段类型
                     * ---------------------------------------
                     * UserID        |  用户标识    |       int(表的主键，自增)
                     * UserSex       |  用户型别    |       int(默认为0，)
                     * IconIdx       |  用户头像    |       int(默认为0，)
                     * NickName      |  用户昵称    |     string
                     * Account       |  登陆账号    |     string
                     * PassWord      |  账户密码    |     string
                     * SigninTime    |  注册时间    |      long
                     * 
                     */
                    m_SQLData.CreateTable("UserInfo", new string[] {
                        "UserID",
                        "UserSex",
                        "IconIdx",
                        "NickName",
                        "Account",
                        "PassWord",
                        "SigninTime",
                    }, new string[] {
                        "INTEGER NOT NULL PRIMARY KEY",
                        "INTEGER DEFAULT 0",
                        "INTEGER DEFAULT 0",
                        "TEXT NOT NULL",
                        "TEXT NOT NULL",
                        "TEXT NOT NULL",
                        "INTEGER",
                    });
                }
            }
        }


        #endregion

        #region 用户信息表的调用

        /// <summary>
        /// 登陆检测
        /// </summary>
        /// <param name="varAccount">账号</param>
        /// <param name="varPassWord">密码</param>
        /// <returns>0-成功</returns>
        public byte User_Detection_AccountAndPassWord(string varAccount, string varPassWord, out uint varUserID)
        {
            varUserID = 0;
            try
            {
                long TempUserID = -1;
                m_SQLData.SelectInfo("UserInfo", (sql) =>
                {
                    while (sql.Read())
                    {
                        string pass = sql["PassWord"].ToString();
                        if (pass != varPassWord)
                            break;
                        TempUserID = uint.Parse(sql["UserID"].ToString());
                        break;
                    }
                }, "*", "Account='" + varAccount+"'");
                if (TempUserID < 0)
                    return 16;          //账号不存在或密码错误
                varUserID = (uint)TempUserID;
            }
            catch(System.Exception ex)
            {
                Debug.LogError(ex.Message);
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 账号检测
        /// </summary>
        /// <param name="varAccount"></param>
        /// <returns>3-账号不存在  1-数据库异常  2-账号已存在</returns>
        public byte User_Detection_Account(string varAccount)
        {
            try
            {
                long TempUserID = -1;
                m_SQLData.SelectInfo("UserInfo", (sql) =>
                {
                    while (sql.Read())
                    {
                        TempUserID = 0;
                        break;
                    }
                }, "*", "Account='" + varAccount+"'");
                if (TempUserID >= 0)
                    return 2;       //账号已存在
                else
                    return 3;       //账号不存在
            }
            catch
            {
                return 1;           //数据库异常
            }
            //return 2;               //账号不存在
        }
        /// <summary>
        /// 昵称检测
        /// </summary>
        /// <param name="varName"></param>
        /// <returns>15-昵称不存在  1-数据库异常  4-昵称已存在</returns>
        public byte User_Detection_NickName(string varName)
        {
            try
            {
                if (string.IsNullOrEmpty(varName))
                    return 10;  //昵称不能为空
                long TempUserID = -1;
                m_SQLData.SelectInfo("UserInfo", (sql) =>
                {
                    while (sql.Read())
                    {
                        TempUserID = 0;
                        break;
                    }
                }, "*", "NickName='" + varName+"'");
                if (TempUserID < 0)
                    return 15;  //昵称不存在
                else
                    return 4;   //昵称已存在
            }
            catch
            {
                return 1;       //数据库异常
            }
        }
        /// <summary>
        /// 注册检测
        /// </summary>
        /// <param name="varAccount">账号</param>
        /// <param name="varPassWord">密码</param>
        /// <param name="varSex">性别</param>
        /// <param name="varNickName">昵称</param>
        /// <returns>0-注册成功</returns>
        public byte User_Detection_Sigin(string varAccount, string varPassWord, byte varSex, string varNickName)
        {
            try
            {
                byte xcode = 0;

                xcode = User_Detection_Account(varAccount);
                if (xcode != 3)
                    return xcode;
                xcode = User_Detection_NickName(varNickName);
                if (xcode !=15)
                    return xcode;

                string content = string.Format("(UserSex,Account,PassWord,NickName) VALUES ({0},'{1}','{2}','{3}')", varSex, varAccount, varPassWord, varNickName);

                m_SQLData.InsertInfo("UserInfo", content);
            }
            catch
            {
                return 1;   //数据库异常
            }
            return 0;       //注册成功
        }
        #endregion

        #region 固定信息数据库

        /// <summary>
        /// 获取错误码信息
        /// </summary>
        /// <param name="ErrorCode"></param>
        /// <returns></returns>
        public string GetErrorCode(int ErrorCode)
        {
            try
            {
                string TempErrorMsg = null;

                if (m_Const_SQLData.ExistsTable("ErrorCode"))
                {
                    m_Const_SQLData.SelectInfo("ErrorCode", (sql) =>
                    {
                        while (sql.Read())
                        {
                            TempErrorMsg = sql["Msg"].ToString();
                        }
                    }, "Msg", "CodeID=" + ErrorCode);
                }
                if (!string.IsNullOrEmpty(TempErrorMsg))
                    return TempErrorMsg;
                else
                    return "错误码不存在" + ErrorCode.ToString();
            }
            catch(System.Exception ex)
            {
                return "数据库操作异常" + ex.Message;
            }
        }

        #endregion

    }
}