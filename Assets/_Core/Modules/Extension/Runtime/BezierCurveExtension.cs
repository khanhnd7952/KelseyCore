using UnityEngine;

namespace Kelsey.Extension
{
    public static class BezierCurveExtension
    {
        public static Vector3[] GetQuadraticBezierPoints(Vector3 startPoint, Vector3 endPoint, float curveHeight)
        {
            var dir = endPoint - startPoint;
            var dir2 = dir.ToVector2().Rotate(90);
            var centerPoint = Vector3.Lerp(startPoint, endPoint, 0.2f);
            Vector3 heighPoint = centerPoint + dir2.ToVector3().normalized * curveHeight;

            Vector3[] res = new Vector3[6];
            float maxT = 1f;
            int index = 0;

            for (float t = 0; t <= maxT; t += 0.2f)
            {
                Vector3 newPoint = (Mathf.Pow(1 - t, 2) * startPoint) + (2 * (1 - t) * t * heighPoint) +
                                   (t * t * endPoint);
                try
                {
                    res[index++] = newPoint;
                }
                catch
                {
                    break;
                }
            }

            return res;
        }
    }
}