using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Manager.Enum;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PG.Manager
{
    public class WindowManager : Manager<WindowManager>
    {
        Dictionary<WindowEnum, string> All_Windows_Path = new Dictionary<WindowEnum, string>();
        Dictionary<WindowEnum, GameObject> All_Windows_Game = new Dictionary<WindowEnum, GameObject>();

        
        Dictionary<WindowEnum, Windows> All_Windows = new Dictionary<WindowEnum, Windows>();                //所有打开过窗口
        Dictionary<WindowEnum, Action> All_WindowsCallBack = new Dictionary<WindowEnum, Action>();          //所有窗口的回调信息
        List<Windows> All_OpenWindows = new List<Windows>();                                                //所有一打开的窗口

        Transform WastWindows;      //废弃窗口
        Transform OpenWindows;      //打开窗口
        Transform MaskUI;           //窗口蒙版
        Canvas m_Canvas;            //画布
        Camera m_UICamera;          //UI相机
        GameObject m_WindowsGameobject;


        /// <summary>
        /// 添加窗口信息
        /// </summary>
        /// <param name="enum">窗口类型</param>
        /// <param name="path">窗口地址（Resource路径地址）</param>
        public void AddPath(WindowEnum @enum, string path)
        {
            if (All_Windows_Path.ContainsKey(@enum))
            {
                Debug.LogError("该类型的窗口信息已存在，无法重复添加");
                return;
            }
            if (All_Windows_Game.ContainsKey(@enum))
            {
                Debug.LogError("该类型的窗口信息已存在，无法重复添加");
                return;
            }
            All_Windows_Path.Add(@enum, path);
        }
        /// <summary>
        /// 添加窗口实体信息
        /// </summary>
        /// <param name="enum">窗口类型</param>
        /// <param name="object">窗口信息</param>
        public void AddPath(WindowEnum @enum ,GameObject @object)
        {
            if (All_Windows_Path.ContainsKey(@enum))
            {
                Debug.LogError("该类型的窗口信息已存在，无法重复添加");
                return;
            }
            if (All_Windows_Game.ContainsKey(@enum))
            {
                Debug.LogError("该类型的窗口信息已存在，无法重复添加");
                return;
            }
            All_Windows_Game.Add(@enum, @object);
        }
        /// <summary>
        /// 清空窗口预制数据
        /// </summary>
        public void ClearWindowsPath()
        {
            All_Windows_Game.Clear();
            All_Windows_Path.Clear();
        }
        /// <summary>
        /// 清口窗口实例数据
        /// </summary>
        public void ClearWindowsGame()
        {
            foreach (var item in All_Windows)
            {
                if (item.Value.m_Windows != null)
                    GameObject.Destroy(item.Value.m_Windows);
            }
            All_Windows.Clear();
            All_OpenWindows.Clear();
        }

        #region 初始化信息


        public void Init()
        {
            WindowsInit();

            SetCanvas();

            SetWindow();
        }
        /// <summary>
        /// 设置窗口跟目录
        /// </summary>
        protected void WindowsInit()
        {
            if (m_WindowsGameobject == null)
            {
                GameObject TempWindows = new GameObject("UIWindows");
                GameObject.DontDestroyOnLoad(TempWindows);
                TempWindows.transform.position = new Vector3(100000, 100000, 100000);
                TempWindows.transform.localScale = Vector3.one;
                TempWindows.transform.eulerAngles = Vector3.zero;
                m_WindowsGameobject = TempWindows;


                //设置画布响应
                GameObject TempEvent = NewGameObject("EventSystem", m_WindowsGameobject);

                TempEvent.AddComponent<EventSystem>();
                TempEvent.AddComponent<StandaloneInputModule>();
            }
        }
        /// <summary>
        /// 设置画布、相机等信息
        /// </summary>
        protected void SetCanvas()
        {
            if (m_Canvas != null)
                return;

            GameObject TempGameObect = NewGameObject("UICanvas", m_WindowsGameobject);
            TempGameObect.layer = LayerMask.NameToLayer("UI");
            //设置UI相机信息
            Camera TempCamera = NewGameObject<Camera>("UICamera", TempGameObect);
            TempCamera.transform.localPosition = new Vector3(0, 0, -500);
            TempCamera.clearFlags = CameraClearFlags.Nothing;
            TempCamera.cullingMask = 32;    //只渲染UI层
            TempCamera.orthographic = true;
            TempCamera.orthographicSize = 1;
            TempCamera.depth = 100;
            TempCamera.gameObject.AddComponent<FlareLayer>();

            m_UICamera = TempCamera;

            //设置画布信息
            Canvas TempCanvas = TempGameObect.AddComponent<Canvas>();
            TempCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            TempCanvas.worldCamera = TempCamera;
            m_Canvas = TempCanvas;

            CanvasScaler TempCanvasScaler = TempGameObect.AddComponent<CanvasScaler>();
            TempCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            TempCanvasScaler.referenceResolution = new Vector2(1920, 1080);
            TempCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            GraphicRaycaster TempGraphic = TempGameObect.AddComponent<GraphicRaycaster>();
        }
        /// <summary>
        /// 设置窗口预制存放点信心
        /// </summary>
        protected void SetWindow()
        {
            if (m_Canvas != null)
            {
                //设置废弃窗口存放点
                if (WastWindows == null)
                {
                    Transform TempWeast = NewGameObject("WastWindow", m_Canvas.transform);
                    TempWeast.gameObject.SetActive(false);
                    WastWindows = TempWeast;
                }
                //设置激活窗口存放点
                if (OpenWindows == null)
                {
                    Transform TempOpen = NewGameObject("OpenWindow", m_Canvas.transform);
                    TempOpen.gameObject.SetActive(true);
                    OpenWindows = TempOpen;
                }
                //设置蒙版
                if (MaskUI == null)
                {
                    Transform TempMask = NewGameObject("UIMask", OpenWindows);
                    TempMask.gameObject.SetActive(false);

                    Image TempUI = TempMask.gameObject.AddComponent<Image>();
                    TempUI.color = new Color(0, 0, 0, 0.3f);

                    MaskUI = TempMask;
                }
            }
        }
        protected GameObject NewGameObject(string varName,GameObject varParent)
        {
            GameObject TempGame = new GameObject(varName);
            TempGame.transform.SetParent(varParent.transform);
            TempGame.transform.localPosition = Vector3.zero;
            TempGame.transform.localScale = Vector3.one;
            TempGame.transform.localEulerAngles = Vector3.zero;
            TempGame.layer = LayerMask.NameToLayer("UI");

            return TempGame;
        }
        protected Transform NewGameObject(string varName, Transform varParent)
        {
            GameObject TempGame = new GameObject(varName);
            RectTransform TempRect = TempGame.AddComponent<RectTransform>();

            TempRect.pivot = Vector2.one * 0.5f;
            TempRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            TempRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            TempRect.anchorMax = Vector2.one;
            TempRect.anchorMin = Vector2.zero;

            TempGame.transform.SetParent(varParent, false);
            TempGame.transform.localPosition = Vector3.zero;
            TempGame.transform.localScale = Vector3.one;
            TempGame.transform.localEulerAngles = Vector3.zero;
            TempGame.layer = LayerMask.NameToLayer("UI");

            return TempGame.transform;
        }
        protected T NewGameObject<T>(string varName, GameObject varParent) where T:Component
        {
            GameObject TempGame = NewGameObject(varName, varParent);
            //TempGame.transform.SetParent(varParent.transform);
            //TempGame.transform.localPosition = Vector3.zero;
            //TempGame.transform.localScale = Vector3.one;
            //TempGame.transform.localEulerAngles = Vector3.zero;
            //TempGame.layer = LayerMask.NameToLayer("UI");
            T Temp = TempGame.AddComponent<T>();
            return Temp;
        }

        
        #endregion



        #region 公开的窗口管理方法


        /// <summary>
        /// 打开一个窗口
        /// </summary>
        /// <param name="enum">窗口类型</param>
        public GameObject Open_Windows(WindowEnum @enum)
        {
            GameObject Temp_Windows = Open(@enum);
            return Temp_Windows;
        }
        /// <summary>
        /// 打开一个窗口
        /// </summary>
        /// <param name="enum">窗口类型</param>
        /// <param name="Mask">是否需要蒙版</param>
        public GameObject Open_Windows(WindowEnum @enum, bool Mask)
        {
            GameObject Temp_Windows = Open(@enum, Mask);
            return Temp_Windows;
        }
        /// <summary>
        /// 打开一个窗口
        /// </summary>
        /// <param name="enum">窗口类型</param>
        /// <param name="Mask">是否需要蒙版</param>
        /// <param name="Level">是否参与层数计算</param>
        public GameObject Open_Windows(WindowEnum @enum,bool Mask,bool Level)
        {
            GameObject Temp_Windows = Open(@enum, Mask, Level);
            return Temp_Windows;
        }

        /// <summary>
        /// 关闭一个窗口
        /// </summary>
        /// <param name="enum">窗口类型</param>
        public void Close_Windows(WindowEnum @enum)
        {
            Close(@enum);
        }
        /// <summary>
        /// 关闭一个窗口
        /// </summary>
        /// <param name="transform">窗口的实例</param>
        public void Close_Windows(Transform varTran)
        {
            Close(varTran);
        }

        
        #endregion


        #region 内部的窗口处理方法


        protected GameObject Open(WindowEnum @enum, bool Mask = true, bool Level = true)
        {
            Windows windows = GetDisOpenWindows(@enum);
            if (windows == null)
            {
                windows = new Windows();
                windows.m_Windows = GetGameObject(@enum);
                if (windows.m_Windows == null)
                {
                    Debug.LogError("error: not exist windows<" + @enum + ">");
                    return null;
                }
                windows.m_IsMask = Mask;
                windows.m_IsLevel = Level;
                windows.m_WindowType = @enum;
            }
            //存储新窗口信息
            SaveWindows(@enum, windows);
            //将新窗口置顶显示
            LastWindows(windows);
            //控制蒙版
            ShowMaskUI();
            return windows.m_Windows;
        }
        protected void Close(WindowEnum @enum)
        {
            //清理关闭的窗口
            for (int i = 0; i < All_OpenWindows.Count; i++)
            {
                Windows Temp_Window = All_OpenWindows[i];
                if (Temp_Window.m_WindowType == @enum)
                {
                    All_OpenWindows.RemoveAt(i);
                    Temp_Window.m_Windows.transform.SetParent(WastWindows);
                    break;
                }
            }
            //控制蒙版的显示
            ShowMaskUI();
        }
        protected void Close(Transform varTran)
        {
            //清理关闭的窗口
            for (int i = 0; i < All_OpenWindows.Count; i++)
            {
                Windows Temp_Window = All_OpenWindows[i];
                if (Temp_Window.m_Windows.transform == varTran)
                {
                    All_OpenWindows.RemoveAt(i);
                    varTran.SetParent(WastWindows);
                    break;
                }
            }
            //控制蒙版的显示
            ShowMaskUI();
        }

        
        /// <summary>
        /// 获取一个已打开的窗口
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        protected Windows GetDisOpenWindows(WindowEnum @enum)
        {
            if (All_Windows.ContainsKey(@enum))
            {
                Windows TempWindows = null;
                All_Windows.TryGetValue(@enum, out TempWindows);
                return TempWindows;
            }
            return null;
        }
        /// <summary>
        /// 通过窗口类型获取一个新窗口
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        protected GameObject GetGameObject(WindowEnum @enum)
        {
            GameObject TempGame = null;
            if (All_Windows_Path.ContainsKey(@enum))
            {
                string path = "";
                All_Windows_Path.TryGetValue(@enum, out path);
                if (string.IsNullOrEmpty(path))
                    return null;
                GameObject game = Resources.Load<GameObject>(path);
                TempGame = GameObject.Instantiate(game) as GameObject;
                return TempGame;
            }
            if (All_Windows_Game.ContainsKey(@enum))
            {
                GameObject game = null;
                All_Windows_Game.TryGetValue(@enum, out game);
                if (game == null)
                    return null;
                TempGame = GameObject.Instantiate(game) as GameObject;
                string TempName = TempGame.name;
                TempGame.name = game.name;
                return TempGame;
            }
            return null;
        }
        /// <summary>
        /// 保存窗口
        /// </summary>
        /// <param name="enum">窗口类型</param>
        /// <param name="varWindows">窗口实例</param>
        protected void SaveWindows(WindowEnum @enum, Windows varWindows)
        {
            if (All_OpenWindows.Contains(varWindows))
                All_OpenWindows.Remove(varWindows);
            All_OpenWindows.Add(varWindows);
            if (All_Windows.ContainsKey(@enum))
                return;
            All_Windows.Add(@enum, varWindows);
        }
        /// <summary>
        /// 将窗口置顶显示
        /// </summary>
        /// <param name="windows"></param>
        protected void LastWindows(Windows windows)
        {
            if (windows == null)
                return;
            if (windows.m_Windows == null)
                return;
            windows.m_Windows.transform.SetParent(OpenWindows, false);
            windows.m_Windows.SetActive(true);
            windows.m_Windows.transform.localPosition = Vector3.zero;
            windows.m_Windows.transform.localScale = Vector3.one;
            windows.m_Windows.transform.SetAsLastSibling();
        }
        /// <summary>
        /// 显示蒙版UI
        /// </summary>
        protected void ShowMaskUI()
        {
            for (int i = All_OpenWindows.Count - 1; i >= 0; i--)
            {
                Windows TempWindows = All_OpenWindows[i];
                if (TempWindows.m_IsMask)
                {
                    MaskUI.SetSiblingIndex(i);
                    MaskUI.gameObject.SetActive(true);
                    return;
                }
            }
            MaskUI.gameObject.SetActive(false);
        }


        #endregion



        public class Windows
        {
            public WindowEnum m_WindowType;     //窗口类型
            public GameObject m_Windows;        //窗口实列
            public bool m_IsMask;               //窗口是否有蒙版
            public bool m_IsLevel;              //窗口是否参与层数计算
        }
    }
}