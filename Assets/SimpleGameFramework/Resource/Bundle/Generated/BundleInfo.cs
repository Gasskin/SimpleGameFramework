namespace SimpleGameFramework.Resource.Bundle.Generated
{
    public class Bundles
    {
        public static BundleInfo scene_loading_material = new BundleInfo("scene_loading/material.ab",null);
        public static BundleInfo scene_loading_prefab = new BundleInfo("scene_loading/prefab.ab",new [] {scene_loading_material});
        public static BundleInfo scene_loading_prefab_prefab2 = new BundleInfo("scene_loading/prefab/prefab2.ab",null);
        public static BundleInfo scene_playing_material = new BundleInfo("scene_playing/material.ab",null);
        public static BundleInfo scene_playing_prefab = new BundleInfo("scene_playing/prefab.ab",null);
    }

    public class BundleAssets
    {
        public static AssetInfo scene_loading_material_Material1 = new AssetInfo("Material1",Bundles.scene_loading_material);
        public static AssetInfo scene_loading_prefab_Cube = new AssetInfo("Cube",Bundles.scene_loading_prefab);
        public static AssetInfo scene_loading_prefab_prefab2_Quad = new AssetInfo("Quad",Bundles.scene_loading_prefab_prefab2);
        public static AssetInfo scene_playing_material_Material2 = new AssetInfo("Material2",Bundles.scene_playing_material);
        public static AssetInfo scene_playing_prefab_Sphere = new AssetInfo("Sphere",Bundles.scene_playing_prefab);
    }
}
