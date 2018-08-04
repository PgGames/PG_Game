using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PG.Model
{

    public class BaseView : MonoBehaviour
    {
        public BaseModel GetModel { set; get; }
        /// <summary>
        /// 窗口View初始化
        /// </summary>
        public virtual void Init()
        {
            AddListener();
        }
        /// <summary>
        /// 添加事件监听
        /// </summary>
        protected virtual void AddListener()
        {
        }
        /// <summary>
        /// 移除事件监听
        /// </summary>
        public virtual void RemoveListener()
        { }
    }
}