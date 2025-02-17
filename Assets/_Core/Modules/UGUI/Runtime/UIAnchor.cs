using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kelsey.UGUI
{
    [ExecuteInEditMode]
    public class UIAnchor : MonoBehaviour
    {
        public AnchorType anchorType;

        private RectTransform _rectTransform;


        RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        private void Start()
        {
            UpdateAnchor();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Selection.gameObjects.Contains(gameObject)) return;
            UpdateAnchor();
#endif
        }

        void UpdateAnchor()
        {
            switch (anchorType)
            {
                case AnchorType.TopLeft:
                    SetRectTransform(new Vector2(0f, 1f));
                    break;
                case AnchorType.Top:
                    SetRectTransform(new Vector2(0.5f, 1f));
                    break;
                case AnchorType.TopRight:
                    SetRectTransform(new Vector2(1f, 1f));
                    break;
                case AnchorType.Right:
                    SetRectTransform(new Vector2(1f, 0.5f));
                    break;
                case AnchorType.BotRight:
                    SetRectTransform(new Vector2(1f, 0f));
                    break;
                case AnchorType.Bot:
                    SetRectTransform(new Vector2(0.5f, 0f));
                    break;
                case AnchorType.BotLeft:
                    SetRectTransform(new Vector2(0f, 0f));
                    break;
                case AnchorType.Left:
                    SetRectTransform(new Vector2(0f, 0.5f));
                    break;
                case AnchorType.Center:
                    SetRectTransform(new Vector2(0.5f, 0.5f));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Rename();
        }

        void SetRectTransform(Vector2 anchor)
        {
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;

            rectTransform.pivot = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        void Rename()
        {
            name = $"*{anchorType}*";
        }
    }

    public enum AnchorType
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BotLeft,
        Bot,
        BotRight,
    }
}