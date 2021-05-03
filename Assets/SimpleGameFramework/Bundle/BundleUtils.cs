using UnityEngine;

namespace SimpleGameFramework.Bundle
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
            string path = string.Empty;
            
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    path = Application.streamingAssetsPath + "/Windows";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.persistentDataPath + "/IOS";
                    break;
                case RuntimePlatform.Android:
                    path = Application.persistentDataPath + "/Android";
                    break;
            }
            
            return path;
        }
    }
}
