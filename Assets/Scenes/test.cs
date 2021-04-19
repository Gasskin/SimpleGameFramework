using SimpleGameFramework.Core;
using SimpleGameFramework.ReferencePool;
using SimpleGameFramework.UI;
using UnityEngine;


public class TestRef : IReference
{
    public string testName="引用池测试";
    public void Clear()
    {
        Debug.Log("TestRef被清空了");
    }
}

public class test : MonoBehaviour
{
    private ReferenceManager referenceManager;
    void Start()
    {
        referenceManager = SGFEntry.Instance.GetManager<ReferenceManager>();
        var tempRef = referenceManager.Acquire<TestRef>();
        Debug.Log(tempRef.testName);
        referenceManager.Release(tempRef);
    }
}


