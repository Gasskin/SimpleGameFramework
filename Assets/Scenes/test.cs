using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AssetBundle ab = AssetBundle.LoadFromFile("AssetBundles/prefab/test.cube");
        GameObject obj = ab.LoadAsset<GameObject>("BundleCube");
        Instantiate(obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
