using System;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private int count = 0;
    private float time = 0;
    private bool flag = false;
    private int fps = 0;
    
    private void Update()
    {
        count++;
        time += Time.deltaTime;
        if (time > 1.0f)
        {
            fps = count;
            count = 0;
            time = 0;
        }
            
    }

    private void OnGUI()
    {
        GUI.color = Color.green;
        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(0, 0, 200, 100), fps.ToString());
        GUI.color= Color.black;
    }
}
