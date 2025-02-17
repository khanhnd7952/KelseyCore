using UnityEngine;

namespace Kelsey.Extension
{
    [ExecuteInEditMode]
    public class FollowCanvasBounder : UpdateOptimizeMonoBehaviour
    {
        [SerializeField] private CanvasBounder canvasBounder;

        private void Start()
        {
            UpdatePosition();
        }

        void UpdatePosition()
        {
            if(canvasBounder == null) return;
            transform.position = canvasBounder.GetPosition();
        }

        protected override void Update()
        {
            base.Update();

#if UNITY_EDITOR
            UpdatePosition();
#endif
        }

        protected override void DoUpdate()
        {
            UpdatePosition();
        }
    }
}