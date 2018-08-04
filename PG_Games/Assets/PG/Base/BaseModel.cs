using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PG.Model
{
    public class BaseModel
    {

        public BaseModel() { }

        protected BaseView m_View;

        //模板初始化
        public virtual void Init() { }

        public virtual void Open() {
            if (m_View != null)
            {
                m_View.Init();
                m_View.GetModel = this;
            }

        }
        public virtual void Close()
        {
            //if (m_View != null)
            //    m_View.RemoveListener();
        }
    }
}