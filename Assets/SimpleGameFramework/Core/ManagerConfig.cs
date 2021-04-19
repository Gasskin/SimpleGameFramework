namespace SimpleGameFramework.Core
{
    public static class ManagerConfig
    {
        /// 引用池的清理间隔
        private const int clearInterval = 60;
        public static int ClearInterval
        {
            get { return clearInterval; }
        }
    }
}
