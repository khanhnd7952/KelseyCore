using System.Collections;
using UnityEngine;

namespace Kelsey
{
    public class App : MonoBehaviour
    {
        static App _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoCreate()
        {
            if (_instance == null)
            {
                var go = new GameObject("App");
                _instance = go.AddComponent<App>();
                DontDestroyOnLoad(go);
            }
        }

        public static Coroutine StartRoutine(IEnumerator routine)
        {
            return _instance.StartCoroutine(routine);
        }
    }
}