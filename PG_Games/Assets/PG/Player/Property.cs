using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PG.PLayer
{
    /// <summary>
    /// 基本属性
    /// </summary>
    public struct Property_Base
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public uint m_ID { set; get; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string m_NickName { set; get; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public byte m_Sex { set; get; }
        /// <summary>
        /// 用户头像url
        /// </summary>
        public string m_Icon { set; get; }
        /// <summary>
        /// 用户头像标识
        /// </summary>
        public ushort m_Icon_Idx { set; get; }

    }
    /// <summary>
    /// 攻击属性
    /// </summary>
    public struct Property_ATK
    {
        /// <summary>
        /// 攻击力
        /// </summary>
        public long m_ATK { set; get; }
        /// <summary>
        /// 魔法攻击力
        /// </summary>
        public long m_ATK_MAGIC { set; get; }
        /// <summary>
        /// 防御力
        /// </summary>
        public long m_DEF { set; get; }
        /// <summary>
        /// 魔法防御力
        /// </summary>
        public long m_DEF_MAGIC { set; get; }
        /// <summary>
        /// 生命值
        /// </summary>
        public long m_HP { set; get; }
        /// <summary>
        /// 魔法值
        /// </summary>
        public long m_MP { set; get; }
        /// <summary>
        /// 经验值
        /// </summary>
        public long m_EXP { set; get; }
        /// <summary>
        /// 升级所需经验值
        /// </summary>
        public long m_UPEXP { set; get; }
        /// <summary>
        /// 等级
        /// </summary>
        public long m_GRADE { set; get; }
    }
    /// <summary>
    /// 财富属性
    /// </summary>
    public struct Property_Money
    {
        /// <summary>
        /// 金币
        /// </summary>
        public long m_Gold { set; get; }
        /// <summary>
        /// 钻石
        /// </summary>
        public long m_Masonry { set; get; }
        /// <summary>
        /// 积分
        /// </summary>
        public long m_Score { set; get; }
    }
}