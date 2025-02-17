using UnityEngine;

namespace Kelsey
{
    public abstract class ScriptableBase : ScriptableObject, IParse
    {
        public abstract void ParseValue(string data);
        public abstract string GetStringValue();
    }
}