using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.ReferencePool;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(typeof(IReference).FullName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
