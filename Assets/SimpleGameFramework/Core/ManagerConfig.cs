using SimpleGameFramework.Resource.Bundle;
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
            public static string DEFAULT_ATUO_PACKAGED_PATH = BundleUtils.GetBundleResourcePath();

            /// 默认打包的位置，资源文件会被打包到这个路径下
            public static string DEFAULT_PACKAGED_LOCATION = BundleUtils.GetPackageToLocation()+$"/{BundleUtils.GetPlatformName()}";
        }
        
        public static class CriwareManagerConfig
        {
            /// CriwareManager
            // 清理CriwareSource组件池的间隔
            public const float AUDIO_POOL_CLEAR_INTERVAL = 60f;
            // 当CueSheet中没有音乐在播放时，它可以存在的时间，超时之后CueSheet会被卸载
            public const float CUE_SHEET_SURVIVAL_TIME = 10f;
            // 挂有所需组件的Prefab的路径，播放音乐时需要被加载
            public const string COMPONENT_PREFAB_PATH = "Criware/AudioComponent";
            // Prefab下，挂有CriAtom组件的GameObject的名字
            public const string CRIATOM_GAMEOBJECT_NAME = "CriAtom";
            // Prefab下，希望挂载Listener组件的GameObject的名字
            public const string LISTENER_GAMEOBJECT_NAME = "Listener";

            /// GenerateAudioData
            // 搜索路径，Generate时会搜索这个路径下的所有文件夹
            public static readonly string SEARCH_PATH = Application.streamingAssetsPath + "/Audios";
            // 保存路径，保存自动生成的CriwareAudios.cs
            public static readonly string SAVE_PATH = Application.dataPath + "/SimpleGameFramework/Audio/Generated/CriwareAudios.cs";
        }
    }
}
