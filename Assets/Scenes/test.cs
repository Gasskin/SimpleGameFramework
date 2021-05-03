using System;
using System.Collections.Generic;
using SimpleGameFramework.Bundle;
using SimpleGameFramework.Core;
using SimpleGameFramework.DataNode;
using SimpleGameFramework.ObjectPool;
using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        SingleABLoader mat = new SingleABLoader("Scene_Loading/Material.ab");
        SingleABLoader ab = new SingleABLoader("Scene_Loading/Prefab.ab");
        var obj = ab.LoadAsset<GameObject>("Cube");
        Instantiate(obj);
    }
}


