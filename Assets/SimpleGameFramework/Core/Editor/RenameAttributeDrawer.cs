using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleGameFramework.Core.Editor
{
    [CustomPropertyDrawer(typeof(RenameAttribute))]
    public class RenameAttributeDrawer : PropertyDrawer
    {
        private RenameAttribute temp => attribute as RenameAttribute;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position,"332111");
        }
    }
}