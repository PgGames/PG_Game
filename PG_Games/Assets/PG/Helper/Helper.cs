using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PG.Helper
{
    public class Helper : MonoBehaviour
    {
        private static Helper m_Helper;
        public static Helper Help
        {
            get
            {
                if (m_Helper)
                {
                    GameObject Temp_Object = new GameObject("Helper");
                    GameObject.DontDestroyOnLoad(Temp_Object);
                    m_Helper = Temp_Object.AddComponent<Helper>();
                }
                return m_Helper;
            }
        }
        
        /// <summary>
        /// 查找某物体下的Transform
        /// </summary>
        /// <param name="Game"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static Transform FindTran(Transform parent, string Path)
        {
            if (parent == null)
            {
                GameObject TempGame = GameObject.Find(Path);
                if (TempGame == null)
                    return null;
                else
                    return TempGame.transform;
            }
            return parent.transform.Find(Path);
        }
        /// <summary>
        /// 查找某物体下的GameObject
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static GameObject FindGame(Transform parent, string Path)
        {
            if (parent == null)
                return GameObject.Find(Path);
            Transform TempTran = parent.transform.Find(Path);
            if (TempTran == null)
                return null;
            else
            return TempTran.gameObject;
        }
        /// <summary>
        /// 查找物体上的某个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Tran"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static T GetComponent<T>(Transform Tran,string Path) where T:Component
        {
            if (Tran == null)
                return null;
            T TempComponent = null;
            if (string.IsNullOrEmpty(Path))
                TempComponent = Tran.GetComponent<T>();
            else
            {
                Transform TempTran = Tran.Find(Path);
                if (TempTran != null)
                    TempComponent = TempTran.GetComponent<T>();
                else
                    Debug.LogError(string.Format("{0}物体下的 {1} 路径不存在", Tran.name, Path));
            }
            return TempComponent;
        }
        /// <summary>
        /// 查找物体上的某个组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Tran"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static T GetComponent<T>(GameObject Tran, string Path) where T : Component
        {
            if (Tran == null)
                return null;
            T TempComponent = null;
            if (string.IsNullOrEmpty(Path))
                TempComponent = Tran.GetComponent<T>();
            else
            {
                Transform TempTran = Tran.transform.Find(Path);
                if (TempTran != null)
                    TempComponent = TempTran.GetComponent<T>();
                else
                    Debug.LogError(string.Format("{0}物体下的 {1} 路径不存在", Tran.name, Path));
            }
            return TempComponent;
        }
        /// <summary>
        /// 设置物体的激活状态
        /// </summary>
        /// <param name="varTren"></param>
        /// <param name="path"></param>
        /// <param name="varActive"></param>
        public static void SetActive(Transform varTren,string path,bool varActive)
        {
            if (varTren == null)
                Debug.LogError("参数 varTren 为空");
            GameObject TempGame = FindGame(varTren, path);
            if (TempGame != null)
                TempGame.SetActive(varActive);
            else
                Debug.LogError(string.Format("{0}物体下的 {1} 路径不存在", varTren.name, path));
        }
    }
}