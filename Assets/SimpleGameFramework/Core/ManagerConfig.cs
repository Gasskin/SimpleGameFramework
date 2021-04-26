namespace SimpleGameFramework.Core
{
    public static class ManagerConfig
    {
        public static class ReferencePoolConfig
        {
            /// 默认清理间隔
            public const int CLEAR_INTERVAL = 60;
        }
        
        public static class ObjectPoolConfig
        {
            /// 默认对象池容量
            public const int DEFAULT_CAPACITY = int.MaxValue;
     
            /// 默认对象过期秒数
            public const float DEFAULT_EXPIRETIME = float.MaxValue;
        }
    }
}
