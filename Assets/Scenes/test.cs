using System;
using System.Collections.Generic;
using AudioFramework;
using SimpleGameFramework.Audio;
using SimpleGameFramework.Core;
using SimpleGameFramework.DataNode;
using SimpleGameFramework.ObjectPool;
using SimpleGameFramework.Resource;
using SimpleGameFramework.Resource.Bundle;
using SimpleGameFramework.Resource.Bundle.Generated;
using UnityEditor.Experimental;
using UnityEngine;

public class test : MonoBehaviour
{
    private GameObject cube;
    private AssetManager assetManager;
    private void Start()
    {
        AudioCenter.PlayAudio(CriwareAudios.demoproj.bgm_05);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            assetManager.Release(cube);
        }
    }
}


