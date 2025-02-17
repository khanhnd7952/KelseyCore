using UnityEngine;

namespace Kelsey.Extension
{
    public static class GameObjectExtension
    {
        public static GameObject GetChildWithName(this GameObject obj, string name)
        {
            Transform trans = obj.transform;
            Transform childTrans = trans.Find(name);

            if (childTrans != null)
            {
                return childTrans.gameObject;
            }

            return null;
        }
    }
}