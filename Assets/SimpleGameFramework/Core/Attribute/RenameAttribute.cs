using UnityEngine;

namespace SimpleGameFramework.Core
{
    public class RenameAttribute : PropertyAttribute
    {
        public string name;

        public RenameAttribute(string str)
        {
            name = str;
        }
    }
}