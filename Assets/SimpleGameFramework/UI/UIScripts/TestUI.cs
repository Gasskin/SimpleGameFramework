using System.Collections;
using System.Collections.Generic;
using SimpleGameFramework.UI;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : UIBase
{
    private Text text;
    public override void Load()
    {
        text = transform.FindText("Image/Text");
        
        transform.RegisterButton("Button",(() =>
        {
            Debug.Log(123123);
        }));
    }

    public override void UnLoad()
    {
        
    }

    private float time = 0;
    public override void OnUpdate(float deltaTime)
    {
        time += deltaTime;
        if (time < 1f) 
        {
            return;
        }

        text.text = deltaTime.ToString();
        time = 0;
    }

    public override void Show()
    {
        Debug.Log("Test Show!");
    }
}
