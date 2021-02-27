using UnityEngine;

namespace SimpleGameFramework.Core
{
    public class ReadOnlyAttribute:PropertyAttribute
    {
        public string name;
        public ReadOnlyAttribute(string str)
        {
            name = str;
        }
    }
}