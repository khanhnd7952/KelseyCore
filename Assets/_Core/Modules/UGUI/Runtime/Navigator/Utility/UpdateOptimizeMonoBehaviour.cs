using UnityEngine;

namespace Kelsey
{
    public abstract class UpdateOptimizeMonoBehaviour : MonoBehaviour
    {
        private float _lastTimeUpdate;

        protected virtual void Update()
        {
            CheckUpdate();
        }

        void CheckUpdate()
        {
            if (Time.frameCount / GetUpdateRate() != 0) return;
            DoUpdate();
        }

        protected virtual void OnEnable()
        {
            DoUpdate();
        }

        protected abstract void DoUpdate();

        protected virtual int GetUpdateRate() => 60;
    }
}