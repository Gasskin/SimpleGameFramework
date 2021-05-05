using UnityEngine;

namespace SimpleGameFramework.Resource.Bundle
{
    public class BundleUtils
    {
        /// <summary>
        /// 获取需要被打包的资源的根路劲
        /// </summary>
        /// <returns></returns>
        public static string GetBundleResourcePath()
        {
            return Application.dataPath + "/Resources Bundle";
        }

        /// <summary>
        /// 获取打包后的存放路径
        /// </summary>
        /// <returns></returns>
        public static string GetPackageToLocation()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return Application.streamingAssetsPath;
                case RuntimePlatform.IPhonePlayer:
                    return Application.persistentDataPath;
                case RuntimePlatform.Android:
                    return Application.persistentDataPath;
            }
            return null;
        }

        public static string GetPlatformName()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.IPhonePlayer:
                    return "IOS";
                case RuntimePlatform.Android:
                    return "Android";
            }

            return null;
        }

        /// <summary>
        /// 获取Bundle全路径
        /// </summary>
        /// <returns></returns>
        public static string GetFullPath(string relativePath)
        {
            return $"{GetPackageToLocation()}/{GetPlatformName()}/{relativePath}";
        }
    }
}
