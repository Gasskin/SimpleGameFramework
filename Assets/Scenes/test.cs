using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.DataNode;
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
    void Start()
    {
        var dataNodeManager = SGFEntry.Instance.GetManager<DataNodeManager>();
        
        // 绝对路径设置数据
        dataNodeManager.SetData("Player.Property.Hp",100);
        
        // 相对路径设置数据
        var node = dataNodeManager.GetOrAddNode("Player.Message");
        dataNodeManager.SetData("Name", "Logarius", node);
        
        // 直接通过数据节点设置数据
        var exp = dataNodeManager.GetOrAddNode("Player.Message.Exp");
        exp.SetData(1000);

        // 获取数据
        string name = dataNodeManager.GetNode("Player.Message.Name").GetData<string>();
        int hp = dataNodeManager.GetNode("Player.Property.Hp").GetData<int>();
        int exps = dataNodeManager.GetNode("Player.Message.Exp").GetData<int>();

        Debug.Log($"{name} {hp} {exps}");
    }

}


