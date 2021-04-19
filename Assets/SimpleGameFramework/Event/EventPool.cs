using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.ReferencePool;
using UnityEngine;

namespace SimpleGameFramework.Event
{
    /// 事件节点，记录事件的发送者与信息并放入事件队列
    /// 仅用于多线中的事件抛出，保证线程安全
    public class Event<T> where T : GlobalEventArgs
    {
        public Event(object sender,T e)
        {
            Sender = sender;
            EventArgs = e;
        }
        public object Sender { get; private set; }
        public T EventArgs { get; private set; }
    }
    
    /// 事件池，收集同一类事件
    public class EventPool<T> where T : GlobalEventArgs
    {
        #region Private

        /// 事件编码与对应的处理方法
        private Dictionary<int, EventHandler<T>> m_EventHandlers;

        /// 事件队列，维护了所有的事件
        private Queue<Event<T>> m_Events;

        #endregion

        #region 构造方法

        public EventPool()
        {
            m_EventHandlers = new Dictionary<int, EventHandler<T>>();
            m_Events = new Queue<Event<T>>();
        }

        #endregion

        #region Public 接口方法

        /// 订阅事件
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("事件处理方法为空，无法订阅...");
            }
 
            EventHandler<T> eventHandler = null;
            
            // 检查是否获取处理方法失败或获取到的为空
            if (!m_EventHandlers.TryGetValue(id, out eventHandler) || eventHandler == null)
            {
                m_EventHandlers[id] = handler;
            }
            // 不为空，就检查处理方法是否重复了
            else if (Check(id, handler))
            {
                Debug.LogError("ID为:" + (SGFEvents)id + "的事件下已存在这个处理方法:" + nameof(handler) + "...");
            }
            else
            {
                eventHandler += handler;
                m_EventHandlers[id] = eventHandler;
            }
        }
 
        /// 取消订阅事件
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("事件处理方法为空，无法取消订阅...");
            }
 
            if (m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers[id] -= handler;
            }
        }

        /// 按顺序执行所有事件队列里的事件
        public void Update(float time)
        {
            while (m_Events.Count > 0)
            {
                Event<T> e;
                lock (m_Events)
                {
                    e = m_Events.Dequeue();
                }
                HandleEvent(e.Sender,e.EventArgs);
            }
        }
        
        /// 抛出事件（线程安全），会按照先后顺序把事件添加进事件队列，并在下一帧处理
        public void Fire(object sender, T e)
        {
            // 将事件源和事件参数封装为Event加入队列
            Event<T> eventNode = new Event<T>(sender, e);
            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }
        
        /// 抛出事件（线程不安全），抛出之后会立刻执行
        public void FireNow(object sender, T e)
        {
            HandleEvent(sender, e);
        }

        /// 清空事件队列
        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        /// 清空事件池
        public void ShutDown()
        {
            Clear();
            m_EventHandlers.Clear();
        }

        /// 检查某个编码的事件是否存在它对应的处理方法
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("事件处理方法为空...");
            }
 
            EventHandler<T> handlers = null;
            if (!m_EventHandlers.TryGetValue(id, out handlers))
            {
                return false;
            }
 
            if (handlers == null)
            {
                return false;
            }
 
            // 遍历委托里的所有方法
            foreach (EventHandler<T> i in handlers.GetInvocationList())
            {
                if (i == handler)
                {
                    return true;
                }
            }
 
            return false;
        }
        
        #endregion

        #region Private 工具方法

        /// 事件处理
        private void HandleEvent(object sender, T e)
        {
            // 尝试获取事件的处理方法
            int eventId = e.Id;
            EventHandler<T> handlers = null;
            if (m_EventHandlers.TryGetValue(eventId, out handlers))
            {
                if (handlers != null)
                {
                    handlers(sender, e);
                }
                else
                {
                    throw new Exception("事件没有对应处理方法：" + eventId);
                }
            }
            // 归还引用
            // SGFEntry.Instance.GetManager<ReferenceManager>().Release<T>(e);
        }

        #endregion
    }
}