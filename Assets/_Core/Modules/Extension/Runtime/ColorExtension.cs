using UnityEngine;

namespace Kelsey.Extension
{
    public static class ColorExtension
    {
        public static Color ToColor(this string colorCode)
        {
            ColorUtility.TryParseHtmlString(colorCode, out var c);
            return c;
        }
    }
}