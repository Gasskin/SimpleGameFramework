namespace SimpleGameFramework.Resource.Bundle
{
    public class BundleInfo
    {
        public string bundlePath;

        public BundleInfo[] dependencies;

        public BundleInfo(string bundlePath,BundleInfo[] dependencies)
        {
            this.bundlePath = bundlePath;
            this.dependencies = dependencies;
        }
    }
    
    public class AssetInfo
    {
        public string assetName;
        
        public BundleInfo bundle;

        public AssetInfo(string assetName, BundleInfo bundle)
        {
            this.assetName = assetName;
            this.bundle = bundle;
        }
    }
}