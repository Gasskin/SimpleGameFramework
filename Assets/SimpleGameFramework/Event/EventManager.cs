using System;
using SimpleGameFramework.Core;

namespace SimpleGameFramework.Event
{
    public class EventManager : ManagerBase
    {
        #region Private

        /// 事件池，维护一个事件池，其实事件管理器就是对事件池的代理
        private EventPool<GlobalEventArgs> m_EventPool;

        #endregion

        #region 构造方法

        public EventManager()
        {
            m_EventPool = new EventPool<GlobalEventArgs>();
        }

        #endregion
        
        #region Override

        public override int Priority
        {
            get { return ManagerPriority.EventManager.GetHashCode(); }
        }

        public override void Init()
        {
            
        }

        public override void Update(float time)
        {
            m_EventPool.Update(time);
        }

        public override void ShutDown()
        {
            
        }

        #endregion

        #region Public 接口方法

        /// 检查订阅事件处理方法是否存在
        public bool Check(int id, EventHandler<GlobalEventArgs> handler)
        {
            return m_EventPool.Check(id, handler);
        }
 
        /// 订阅事件
        public void Subscribe(SGFEvents id, EventHandler<GlobalEventArgs> handler)
        {
            m_EventPool.Subscribe(id.GetHashCode(), handler);
        }
 
        /// 取消订阅事件
        public void Unsubscribe(SGFEvents id, EventHandler<GlobalEventArgs> handler)
        {
            m_EventPool.Unsubscribe(id.GetHashCode(), handler);
        }
        
        /// 抛出事件（线程安全）
        public void Fire(object sender, GlobalEventArgs e)
        {
            m_EventPool.Fire(sender, e);
        }
        
        /// 抛出事件（线程不安全）
        public void FireNow(object sender, GlobalEventArgs e)
        {
            m_EventPool.FireNow(sender, e);
        }
        #endregion
    }
}