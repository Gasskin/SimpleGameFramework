using SimpleGameFramework.ReferencePool;

namespace SimpleGameFramework.Event
{
    /// 所有事件的基类，会被引用池管理
    public abstract class GlobalEventArgs:IReference
    {
        public abstract int Id { get; set; }
        public abstract void Clear();
    }
}