using SimpleGameFramework.Core;
using SimpleGameFramework.ObjectPool;
using UnityEngine;


public class TestObject : ObjectBase
{
    public TestObject(object target, string name = "") : base(target, name)
    {
    }

    public override void Release()
    {
        
    }
}

public class test : MonoBehaviour
{
    private ObjectPoolManager objectPoolManager;
    private ObjectPool<TestObject> testPool;
    void Start()
    {
        objectPoolManager = SGFEntry.Instance.GetManager<ObjectPoolManager>();
        testPool = objectPoolManager.CreateObjectPool<TestObject>();

        TestObject temp = new TestObject("HelloWorld","test1");
        testPool.Register(temp);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestObject testObject = testPool.Spawn("test1");
            Debug.Log(testObject.Target);
            testPool.Unspawn(testObject.Target);
        }
    }
}


