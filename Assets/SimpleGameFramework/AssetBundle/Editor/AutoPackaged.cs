using System.IO;
using SimpleGameFramework.Core;
using UnityEditor;
using UnityEngine;

namespace SimpleGameFramework.AssetBundle.Editor
{
    public static class AutoPackaged 
    {
        [MenuItem("SGFTools/Asset Bundle/PackagedAll")]
        public static void Package()
        {
            // 清空没有使用过的AB标记
            AssetDatabase.RemoveUnusedAssetBundleNames();
            
            // 搜索根目录
            string searchRoot = ManagerConfig.AssetBundleConfig.DEFAULT_ATUO_PACKAGED_PATH;

            // 获取根目录下的所有文件（包括文件和文件夹），并继续递归搜索
            DirectoryInfo rootDirs = new DirectoryInfo(searchRoot);
            var files = rootDirs.GetDirectories();
            foreach (var file in files)
            {
                // 递归判断当前目录
                IsDirOrFIle(file,file.Name);
            }

            Debug.Log(string.Format("<color=green>AssetBundle自动标记完成！</color>"));
            Debug.Log("开始打包AseetBundle...");

            string dir = ManagerConfig.AssetBundleConfig.DEFAULT_PACKAGED_LOCATION;

            // 每次打包都要把原来的包删掉
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir,true);
            }
            Directory.CreateDirectory(dir);

            BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
            
            Debug.Log(string.Format("<color=green>AssetBundle打包完成！</color>"));
            
            // 刷新
            AssetDatabase.Refresh();
        }

        
        /// <summary>
        /// 判断当前目录是文件夹，还是普通文件
        /// </summary>
        /// <param name="fileSystemInfo">当前目录信息</param>
        /// <param name="sceneName">当前场景名称</param>
        private static void IsDirOrFIle(FileSystemInfo fileSystemInfo, string sceneName)
        {
            if (!fileSystemInfo.Exists)
                return;

            var dirInfo = fileSystemInfo as DirectoryInfo;
            FileSystemInfo[] subDir = dirInfo?.GetFileSystemInfos();

            if (subDir == null) 
                return;
            
            foreach (FileSystemInfo file in subDir)
            {
                // Unity资源信息文件直接跳过
                if (file.Extension.Equals(".meta")) 
                    continue;

                FileInfo fileInfo = file as FileInfo;
                // 说明是文件类型
                if (fileInfo != null)
                {
                    SetAssetBundleLabel(fileInfo, sceneName);
                }
                // 说明是文件夹类型
                else
                {
                    IsDirOrFIle(file, $"{sceneName}/{file.Name}");
                }
            }
        }

        /// <summary>
        /// 给资源文件设置Bundle标签
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="sceneName">所在场景目录</param>
        private static void SetAssetBundleLabel(FileInfo fileInfo, string sceneName)
        {
            int index = fileInfo.FullName.IndexOf("Assets");
            var relativePath = fileInfo.FullName.Substring(index);
            
            // 只能打开Assets下的文件，所以必须或者相对路径
            AssetImporter res = AssetImporter.GetAtPath(relativePath);

            // 标签名
            res.assetBundleName = sceneName;
            
            // 扩展名，如果是场景就定义为u3d
            if (fileInfo.Extension.Equals(".unity"))
            {
                res.assetBundleVariant = "u3d";
            }
            // 否则是ab
            else
            {
                res.assetBundleVariant = "ab";
            }
        }
    }
}
