using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.DataNode;
using SimpleGameFramework.ObjectPool;
using SimpleGameFramework.Resource;
using SimpleGameFramework.Resource.Bundle;
using SimpleGameFramework.Resource.Bundle.Generated;
using UnityEngine;

public class test : MonoBehaviour
{
    private GameObject cube;
    private AssetManager assetManager;
    private void Start()
    {
        assetManager = SGFEntry.Instance.GetManager<AssetManager>();
        cube = assetManager.LoadAsset<GameObject>(BundleAssets.scene_loading_prefab_Cube);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            assetManager.Release(cube);
        }
    }
}


