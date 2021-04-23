using System;
using SimpleGameFramework.Core;
using SimpleGameFramework.Language.Editor;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleGameFramework.Language
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class TextID : MonoBehaviour
    {
        #region Field

        public Text text;
        public LanguageID id;

        private LanguageManager languageManager;
        #endregion

        #region Mono

        private void Awake()
        {
            languageManager = SGFEntry.Instance.GetManager<LanguageManager>();
            
            text = GetComponent<Text>();
            if (text == null)
                throw new Exception("挂在Text组件失败...");
            RefreshText ();
            languageManager.onLanguageChange += RefreshText;
        }

        private void OnDestroy()
        {
            languageManager.onLanguageChange -= RefreshText;
        }

        #endregion
        
        #region Event 方法
    
        private void RefreshText ()
        {
            if (text != null)
                text.text = languageManager.GetText (id);
        }
    
        #endregion
    }
}