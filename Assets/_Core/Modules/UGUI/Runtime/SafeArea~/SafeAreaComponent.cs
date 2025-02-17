using System;
using Kelsey;

#if KELSEY_SOAP
using Obvious.Soap;
#endif

using UnityEngine;

namespace Squirrel.UGUI
{
    public sealed class SafeAreaComponent : SafeAreaBase
    {
        [Flags]
        public enum EPadding
        {
            Top = 1 << 0,
            Bottom = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
        }

        [field: SerializeField, EnumFlags] private EPadding Padding { get; set; } = (EPadding)Enum.Parse(typeof(EPadding), (-1).ToString());
#if KELSEY_SOAP
        [SerializeField] private FloatVariable bannerHeight;
#endif

        void OnDestroy()
        {
#if KELSEY_SOAP
            if (bannerHeight != null)
            {
                bannerHeight.OnValueChanged -= BannerHeightOnOnValueChanged;
                //bannerHeight.Value = 0;
            }
#endif
        }

        void Start()
        {
#if KELSEY_SOAP
            if (bannerHeight != null)
            {
                bannerHeight.OnValueChanged += BannerHeightOnOnValueChanged;
                BannerHeightOnOnValueChanged(bannerHeight.Value);
            }
#endif
        }

        private void BannerHeightOnOnValueChanged(float obj)
        {
            UpdateRect();
        }

        public override void ResetRect()
        {
            base.ResetRect();
            RectT.anchorMin = Vector2.zero;
            RectT.anchorMax = Vector2.one;
        }

        protected override void UpdateRect(Rect safeArea, int width, int height)
        {
            if (safeArea.width.Approximately(width) && safeArea.height.Approximately(height))
            {
                ResetRect();
                return;
            }

            var paddingTop = 0f;
            var paddingRight = 0f;
            var paddingLeft = 0f;
            var paddingBottom = 0f;

            if (Padding.HasFlag(EPadding.Top)) paddingTop = height - (safeArea.height + safeArea.y);
            if (Padding.HasFlag(EPadding.Right)) paddingRight = width - (safeArea.width + safeArea.x);
            if (Padding.HasFlag(EPadding.Bottom)) paddingBottom = safeArea.y;
            if (Padding.HasFlag(EPadding.Left)) paddingLeft = safeArea.x;

#if KELSEY_SOAP
            if (bannerHeight != null)
            {
                paddingBottom += bannerHeight.Value;
            }
#endif


            RectT.sizeDelta = RectT.anchoredPosition = Vector3.zero;
            RectT.anchorMin = new Vector2(paddingLeft / width, paddingBottom / height);
            RectT.anchorMax = new Vector2((width - paddingRight) / width, (height - paddingTop) / height);
        }
    }
}