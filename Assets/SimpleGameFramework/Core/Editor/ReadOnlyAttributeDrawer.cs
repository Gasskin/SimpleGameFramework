using UnityEditor;
using UnityEngine;

namespace SimpleGameFramework.Core.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        private string value;
        private ReadOnlyAttribute attr => attribute as ReadOnlyAttribute;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    value = property.intValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    value = (property.floatValue * 1000f).ToString() + "ms";
                    break;
            }
            EditorGUI.LabelField(position, attr.name + "  :  " + value);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}