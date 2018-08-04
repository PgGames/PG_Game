using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PG.PLayer
{

    public class Player
    {
        protected Property_Base m_Base;
        protected Property_Money m_Money;

        /// <summary>
        /// 获取财富信息
        /// </summary>
        public Property_Money GetMoney
        {
            get { return m_Money; }
        }
        /// <summary>
        /// 获取基础信息
        /// </summary>
        public Property_Base GetProperty
        {
            get { return m_Base; }
        }

    }
}