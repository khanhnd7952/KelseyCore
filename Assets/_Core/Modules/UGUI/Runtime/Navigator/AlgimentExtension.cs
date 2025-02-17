using System;
using UnityEngine;

namespace Kelsey.UGUI
{
    internal static class AlgimentExtension
    {
        public static Vector3 ToPosition(this EAlignment alignment, RectTransform rectTransform)
        {
            var rect = rectTransform.rect;
            float width = rect.width;
            float height = rect.height;
            float z = rectTransform.localPosition.z;
            var position = alignment switch
            {
                EAlignment.Left => new Vector3(-width, 0, z),
                EAlignment.Right => new Vector3(width, 0, z),
                EAlignment.Top => new Vector3(0, height, z),
                EAlignment.Bottom => new Vector3(0, -height, z),
                EAlignment.Center => new Vector3(0, 0, z),
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
            };

            return position;
        }
    }
}