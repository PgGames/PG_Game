using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PG.Manager.Enum;
using System;

namespace PG.Manager
{
    public class EventObjet{ }
    public class EventClass<T> : EventObjet
    {
        private T m_T;
        public EventClass(T varT)
        {
            m_T = varT;
        }
        public T GetT { get { return m_T; } }
    }
    public class EventClass<T, U> : EventObjet
    {
        private T m_T;
        private U m_U;
        public EventClass(T varT, U varU)
        {
            m_T = varT;
            m_U = varU;
        }
        public T GetT { get { return m_T; } }
        public U GetU { get { return m_U; } }
    }
    public class EventClass<T, U, V> : EventObjet
    {
        private T m_T;
        private U m_U;
        private V m_V;
        public EventClass(T varT, U varU,V varV)
        {
            m_T = varT;
            m_U = varU;
            m_V = varV;
        }
        public T GetT { get { return m_T; } }
        public U GetU { get { return m_U; } }
        public V GetV { get { return m_V; } }
    }
    public delegate void EventMonth(EventObjet @event);

    public class EventManager : Manager<EventManager>
    {
        protected static Dictionary<EventEnum, List<EventMonth>> All_Event = new Dictionary<EventEnum, List<EventMonth>>();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="enum">事件类型</param>
        /// <param name="event">事件响应回调</param>
        public void AddListener(EventEnum @enum, EventMonth @event)
        {
            List<EventMonth> TempDate;
            All_Event.TryGetValue(@enum, out TempDate);
            if (TempDate == null)
                TempDate = new List<EventMonth>();

            if (TempDate.Contains(@event))
            {
                throw new System.Exception("该监听已存在，无法再次添加");
            }
            else
            {
                TempDate.Add(@event);
            }
        }
        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="enum">事件类型</param>
        /// <param name="event">事件响应回调</param>
        public void RemoveListener(EventEnum @enum, EventMonth @event)
        {
            List<EventMonth> TempDate;
            All_Event.TryGetValue(@enum, out TempDate);
            if (TempDate == null)
            {
                throw new System.Exception("该监听不存在，无法取消监听");
            }

            if (!TempDate.Contains(@event))
            {
                throw new System.Exception("该方法的监听不存在，无法取消监听");
            }
            else
            {
                TempDate.Remove(@event);
            }
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="enum">事件类型</param>
        /// <param name="event">事件的数据</param>
        public static void Broadcasts(EventEnum @enum, EventObjet @event)
        {
            GetManager().Broad(@enum, @event);
        }
        /// <summary>
        /// 广播事件（参数默认为空）
        /// </summary>
        /// <param name="enum">事件类型</param>
        public static void Broadcast(EventEnum @enum)
        {
            GetManager().Broad(@enum, null);
        }
        /// <summary>
        /// 广播事件
        /// </summary>
        /// <typeparam name="T">参数1的类型</typeparam>
        /// <param name="enum">事件类型</param>
        /// <param name="event">事件参数</param>
        public static void Broadcast<T>(EventEnum @enum, T @event)
        {
            EventObjet temp_event = new EventClass<T>(@event);
            GetManager().Broad(@enum, temp_event);
        }
        /// <summary>
        /// 广播事件
        /// </summary>
        /// <typeparam name="T">参数1的类型</typeparam>
        /// <typeparam name="U">参数2的类型</typeparam>
        /// <param name="enum">事件类型</param>
        /// <param name="eventT">事件参数1</param>
        /// <param name="eventU">事件参数2</param>
        public static void Broadcast<T, U>(EventEnum @enum, T @eventT, U @eventU)
        {
            EventObjet temp_event = new EventClass<T, U>(eventT, eventU);
            GetManager().Broad(@enum, temp_event);
        }
        /// <summary>
        /// 广播事件
        /// </summary>
        /// <typeparam name="T">参数1的类型</typeparam>
        /// <typeparam name="U">参数2的类型</typeparam>
        /// <typeparam name="V">参数3的类型</typeparam>
        /// <param name="enum">事件类型</param>
        /// <param name="eventT">事件参数1</param>
        /// <param name="eventU">事件参数2</param>
        /// <param name="eventV">事件参数3</param>
        public static void Broadcast<T, U, V>(EventEnum @enum, T @eventT, U @eventU, V @eventV)
        {
            EventObjet temp_event = new EventClass<T, U, V>(eventT, eventU, eventV);
            GetManager().Broad(@enum, temp_event);
        }
        /// <summary>
        /// 解析事件参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="event">事件数据</param>
        /// <param name="varT">事件参数1的解析结果</param>
        public static void GetValue<T>(EventObjet @event,out T varT)
        {
            EventClass<T> TempDate = @event as EventClass<T>;
            varT = TempDate.GetT;
        }
        /// <summary>
        /// 解析事件参数
        /// </summary>
        /// <typeparam name="T">参数1类型</typeparam>
        /// <typeparam name="U">参数2类型</typeparam>
        /// <param name="event">事件数据</param>
        /// <param name="varT">事件参数1的解析结果</param>
        /// <param name="varU">事件参数2的解析结果</param>
        public static void GetValue<T, U>(EventObjet @event, out T varT, out U varU)
        {
            EventClass<T, U> TempDate = @event as EventClass<T, U>;
            varT = TempDate.GetT;
            varU = TempDate.GetU;
        }
        /// <summary>
        /// 解析事件参数
        /// </summary>
        /// <typeparam name="T">参数1类型</typeparam>
        /// <typeparam name="U">参数2类型</typeparam>
        /// <typeparam name="V">参数3类型</typeparam>
        /// <param name="event">事件数据</param>
        /// <param name="varT">事件参数1的解析结果</param>
        /// <param name="varU">事件参数2的解析结果</param>
        /// <param name="varV">事件参数3的解析结果</param>
        public static void GetValue<T, U, V>(EventObjet @event, out T varT, out U varU, out V varV)
        {
            EventClass<T, U, V> TempDate = @event as EventClass<T, U, V>;
            varT = TempDate.GetT;
            varU = TempDate.GetU;
            varV = TempDate.GetV;
        }

        /// <summary>
        /// 处理事件信息
        /// </summary>
        /// <param name="enum">事件类型</param>
        /// <param name="event">事件数据</param>
        protected void Broad(EventEnum @enum, EventObjet @event)
        {
            if (All_Event.ContainsKey(@enum))
            {
                List<EventMonth> TempDate;
                if (All_Event.TryGetValue(@enum, out TempDate))
                {
                    for (int i = 0; i < TempDate.Count; i++)
                    {
                        if (TempDate[i] != null)
                            TempDate[i](@event);
                    }
                }
                else
                    Debug.LogError("error: not exist " + @enum + " Listener");
            }
            else
                Debug.LogError("error: not exist " + @enum + " Listener");
        }
    }

}