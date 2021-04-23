using System;
using System.Collections.Generic;
using SimpleGameFramework.Core;
using SimpleGameFramework.Event;
using SimpleGameFramework.Language;
using SimpleGameFramework.Language.Editor;
using SimpleGameFramework.Procedure;
using SimpleGameFramework.ReferencePool;
using SimpleGameFramework.UI;
using UnityEngine;



public class test : MonoBehaviour
{
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SGFEntry.Instance.GetManager<LanguageManager>().SetCurrentLanguage(SystemLanguage.English);
        }

        if (Input.GetMouseButtonDown(1))
        {
            SGFEntry.Instance.GetManager<LanguageManager>().SetCurrentLanguage(SystemLanguage.ChineseSimplified);
        }
    }
}


