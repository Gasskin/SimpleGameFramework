using System;
using System.Collections.Generic;
using System.Net.Mime;
using SimpleGameFramework.Language.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleGameFramework.Language
{
    [CustomEditor(typeof(TextID))]
    public class TextIDEditor : UnityEditor.Editor
    {
        /// <summary>
        /// 添加Text组件
        /// </summary>
        [MenuItem ("GameObject/UI/TextID")]
        private static void CreateTextMultiLanguage ()
        {
            var go = new GameObject {name = "Text"};
            go.transform.SetParent (Selection.activeTransform, false);
            go.AddComponent<TextID> ();
            Selection.activeObject = go;
        }
        
        private TextID inst => target as TextID;


        private Dictionary<LanguageID, string> textData;

        protected void OnEnable ()
        {
            textData = new Dictionary<LanguageID, string> ();
            new LanguageReader ().Read (textData, SystemLanguage.ChineseSimplified.ToString());
            inst.text = inst.gameObject.GetComponent<Text> ();
        }

        public override void OnInspectorGUI()
        {
            inst.id = (LanguageID)EditorGUILayout.EnumPopup("ID", inst.id);
            
            if (GUI.changed)
            {
                if (inst.text != null)
                    inst.text.text = textData[inst.id];
                
                EditorUtility.SetDirty(target);
            }
        }
    }
}