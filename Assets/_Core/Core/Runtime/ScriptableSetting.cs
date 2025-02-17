using System;
using UnityEngine;

namespace Kelsey
{
    public abstract class ScriptableSetting<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T gInstance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = Resources.Load<T>(typeof(T).Name);
                if (_instance == null)
                    throw new Exception($"Scriptable setting for {typeof(T)} must be create before use!");
                return _instance;
            }
        }

        public static bool IsExist() => Resources.Load<T>(typeof(T).Name) != null;
    }
}