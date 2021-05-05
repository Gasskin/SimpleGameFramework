using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SimpleGameFramework.Resource.Bundle.Editor
{
    
    
    public class MainfestAnalyse
    {
        [MenuItem("SGFTools/Asset Bundle/Generate BundleInfo.cs")]
        public static void Analyse()
        {
            // Bundle根路径
            string rootpath = BundleUtils.GetPackageToLocation() + $"/{BundleUtils.GetPlatformName()}";
            // manifest路径
            string manifestPath = rootpath + $"/{BundleUtils.GetPlatformName()}";
            // 根据manifest获取所有的Bundle
            AssetBundle manifestBundle = AssetBundle.LoadFromFile(manifestPath);
            AssetBundleManifest manifest = manifestBundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
            var allAssetBundles = manifest.GetAllAssetBundles();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace SimpleGameFramework.Resource.Bundle.Generated");
            sb.AppendLine("{");
            sb.AppendLine("    public class Bundles");
            sb.AppendLine("    {");
            
            // 遍历所有Bundle
            foreach (var bundle in allAssetBundles)
            {
                int index = bundle.IndexOf(".");
                var tempBundlename = bundle.Substring(0, index);
                var bundleName = tempBundlename.Replace("/", "_");
                
                // 当前Bundle的依赖Bunlde
                var tempDependencies = manifest.GetAllDependencies(bundle);
                
                string dependencies = "new [] {";
                int i = 0;
                foreach (var dependency in tempDependencies)
                {
                    if (i++ > 0)
                        dependencies += ",";
                    index = dependency.IndexOf(".");
                    var tempDependency = dependency.Substring(0, index);
                    var dependencyName = tempDependency.Replace("/", "_");
                    dependencies += dependencyName;
                }
                
                dependencies += "}";

                if (i <= 0)
                    dependencies = "null";
                
                sb.AppendLine($"        public static BundleInfo {bundleName} = new BundleInfo(\"{bundle}\",{dependencies});");

            }
            manifestBundle.Unload(true);

            sb.AppendLine("    }");
            sb.AppendLine();
            sb.AppendLine("    public class BundleAssets");
            sb.AppendLine("    {");
            
            // 遍历每个Bundle的所有资源
            foreach (var bundle in allAssetBundles)
            {
                int index = bundle.IndexOf(".");
                var tempBundlename = bundle.Substring(0, index);
                var bundleName = tempBundlename.Replace("/", "_");
                
                var assets = AssetDatabase.GetAssetPathsFromAssetBundle(bundle);
                foreach (var asset in assets)
                {
                    var assetName = Path.GetFileNameWithoutExtension(asset);
                    sb.AppendLine($"        public static AssetInfo {bundleName}_{assetName} = new AssetInfo(\"{assetName}\",Bundles.{bundleName});");
                }
            }
            
            sb.AppendLine("    }");
            
            sb.AppendLine("}");

            File.WriteAllText(Application.dataPath + "/SimpleGameFramework/Resource/Bundle/Generated/BundleInfo.cs", sb.ToString());
            
            AssetDatabase.Refresh();
        }
    }
}
