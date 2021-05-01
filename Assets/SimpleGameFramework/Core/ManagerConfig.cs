using UnityEngine;

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

        public static class AssetBundleConfig
        {
            /// 默认需要打包的路径，这个路径下的所有文件会被自动打包为Bundle，依据文件夹分组  
            public static string DEFAULT_ATUO_PACKAGED_PATH = Application.dataPath + "/Resources Bundle";
            
            /// 默认打包的位置，资源文件会被打包到这个路径下
            public const string DEFAULT_PACKAGED_LOCATION = "Assets/AssetsBundle";
        }
    }
}
