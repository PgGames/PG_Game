using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Model;
using PG.Manager.Enum;

namespace PG.Manager
{

    public class ModelManager : Manager<ModelManager>
    {
        #region 外部接口

        public void Init()
        {
            Protected_Init();

            InitModel();
        }
        /// <summary>
        /// 初始化Model
        /// </summary>
        public void InitModel()
        {
            foreach (var item in All_Model)
            {
                item.Value.Init();
            }
        }

        public void Open(ModelEnum modelEnum)
        {
            if (!All_Model.ContainsKey(modelEnum))
            {
                Debug.LogError("该类型的model不存在");
                return;
            }
            BaseModel model;
            All_Model.TryGetValue(modelEnum, out model);
            if (model != null)
                model.Open();
        }
        public void Close(ModelEnum modelEnum)
        {
            if (!All_Model.ContainsKey(modelEnum))
            {
                Debug.LogError("该类型的model不存在");
                return;
            }
            BaseModel model;
            All_Model.TryGetValue(modelEnum, out model);
            if (model != null)
                model.Close();
        }

        public T GetModel<T>() where T:BaseModel
        {
            foreach (var item in All_Model.Values)
            {
                if (item.GetType() == typeof(T))
                {
                    return item as T;
                }
            }
            return null;
        }

        #endregion

        //protected List<BaseModel> All_Model = new List<BaseModel>();
        protected Dictionary<ModelEnum, BaseModel> All_Model = new Dictionary<ModelEnum, BaseModel>();

        protected void Protected_Init()
        {
            AddModle(ModelEnum.Login, new LoginModel());
        }
        protected void AddModle(ModelEnum modelEnum, BaseModel varModel)
        {
            if (All_Model.ContainsKey(modelEnum))
                return;
            All_Model.Add(modelEnum, varModel);
        }
    }

}