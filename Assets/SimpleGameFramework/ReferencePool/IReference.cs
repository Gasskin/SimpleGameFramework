namespace SimpleGameFramework.ReferencePool
{
    /// 引用接口，实现这个接口的类会被引用池管理
    public interface IReference
    {
        /// 归还引用时的清理方法
        void Clear();
    }
}


