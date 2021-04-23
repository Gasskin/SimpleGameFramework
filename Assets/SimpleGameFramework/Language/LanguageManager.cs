using System;
using System.Collections.Generic;
using System.Text;
using SimpleGameFramework.Core;
using SimpleGameFramework.Language.Editor;
using UnityEngine;

namespace SimpleGameFramework.Language
{
    public class LanguageManager:ManagerBase
    {
        #region Event
            public event Action onLanguageChange;
        #endregion


        #region Filed

            public const SystemLanguage DEFAULT_LANGUAGE = SystemLanguage.English;
            private string currentLanguage;
            private LanguageReader languageReader;
            private StringBuilder  textMaker;
            
            /// 当前语种的字典，切换语种需要重新加载这个字典
            private Dictionary<LanguageID, string> textData;
            private const string CURRENT_LANGUAGE = "LanguageFramework.Current_Language";
        #endregion
        
        public override int Priority
        {
            get { return ManagerPriority.LanguageManager.GetHashCode(); }
        }

        #region 生命周期

            public override void Init()
            {
                currentLanguage = PlayerPrefs.GetString (CURRENT_LANGUAGE);
                if (string.IsNullOrEmpty(currentLanguage)) 
                    currentLanguage = Application.systemLanguage.ToString();

                textMaker      = new StringBuilder ();
                languageReader = new LanguageReader ();
                textData       = new Dictionary<LanguageID, string> ();
                languageReader.Read (textData, currentLanguage);
            }

            public override void Update(float time)
            {
            }

            public override void ShutDown()
            {
            }

        #endregion
        
        #region Language Method
        /// <summary>
        /// 设置语种，并刷新页面上的文字 
        /// </summary>
        public void SetCurrentLanguage (SystemLanguage newLanguage)
        {
            var language = newLanguage.ToString ();
            if (currentLanguage == language)
                return;

            PlayerPrefs.SetString (CURRENT_LANGUAGE, language);
            currentLanguage = language;
            textData.Clear ();
            languageReader.Read (textData, language);
            onLanguageChange?.Invoke ();
        }
        #endregion
        
        #region Text Method
        /// <summary>
        /// 获取与 TextID 对应的 String ，语种等于当前 language
        /// </summary>
        public string GetText (LanguageID id)
        {
            if (textData.TryGetValue (id, out var content))
                return content;

            return "#Invalid#";
        }

        /// <summary>
        /// 获取与 TextID 对应的 String ，语种等于当前 language
        /// </summary>
        public string GetText (LanguageID id, object arg0)
        {
            if (textData.TryGetValue (id, out var content))
            {
                textMaker.Clear ();
                textMaker.AppendFormat (content, arg0);
                return textMaker.ToString ();
            }

            return "#Invalid#";
        }

        /// <summary>
        /// 获取与 TextID 对应的 String ，语种等于当前 language
        /// </summary>
        public string GetText (LanguageID id, object arg0, object arg1)
        {
            if (textData.TryGetValue (id, out var content))
            {
                textMaker.Clear ();
                textMaker.AppendFormat (content, arg0, arg1);
                return textMaker.ToString ();
            }

            return "#Invalid#";
        }

        /// <summary>
        /// 获取与 TextID 对应的 String ，语种等于当前 language
        /// </summary>
        public string GetText (LanguageID id, object arg0, object arg1, object arg2)
        {
            if (textData.TryGetValue (id, out var content))
            {
                textMaker.Clear ();
                textMaker.AppendFormat (content, arg0, arg1, arg2);
                return textMaker.ToString ();
            }

            return "#Invalid#";
        }

        /// <summary>
        /// 获取与 TextID 对应的 String ，语种等于当前 language
        /// </summary>
        public string GetText (LanguageID id, params object[] args)
        {
            if (textData.TryGetValue (id, out var content))
            {
                textMaker.Clear ();
                textMaker.AppendFormat (content, args);
                return textMaker.ToString ();
            }

            return "#Invalid#";
        }
    #endregion
    }
}