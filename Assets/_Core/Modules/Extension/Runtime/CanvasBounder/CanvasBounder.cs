using UnityEngine;

namespace Kelsey.Extension
{
    public class CanvasBounder : MonoBehaviour
    {
        private Canvas _canvas;
        [SerializeField] private Transform top, bot, left, right;
        public Canvas CanvasParent
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = GetComponentInParent<Canvas>();
                }

                return _canvas;
            }
        }

        public Bounds GetBounds()
        {
            if (CanvasParent == null) return new Bounds();
            return new Bounds(GetPosition(), GetSize());
        }

        public Vector2 GetSize()
        {
            if (CanvasParent == null) return Vector2.zero;
            return (transform as RectTransform).CanvasObjectToWorldSize(CanvasParent);
        }

        public Vector3 GetPosition()
        {
            if (CanvasParent == null) return Vector3.zero;
            var position = transform.CanvasObjectToWorldPosition(CanvasParent);
            position.z = 0f;
            return position;
        }

        public Vector2 GetScreenPoint()
        {
            return RectTransformUtility.WorldToScreenPoint(CanvasParent.worldCamera, GetPosition());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var bound = GetBounds();
            Gizmos.DrawWireCube(bound.center, bound.size);
        }

        public Vector3 GetTopPosition() => top.position;
        public Vector3 GetBotPosition() => bot.position;
        public Vector3 GetLeftPosition() => left.position;
        public Vector3 GetRightPosition() => right.position;
        public Vector3 GetCenterPosition() => transform.position;
    }
}