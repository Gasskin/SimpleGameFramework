using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.UI;
using UnityEngine;
using UnityEngine.UI;

public class Update2 : UIBase
{
    private UIManager manager;

    private Text text;
    
    public override void Load()
    {
        UIType = UIType.PopUp;

        manager = SGFEntry.Instance.GetManager<UIManager>();

        text = transform.FindText("Image/Text");
        
        transform.RegisterButton("Image/Button",(() =>
        {
            manager.CloseCurrent();
        }));
    }

    public override void UnLoad()
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        text.text = deltaTime.ToString();
    }
}
