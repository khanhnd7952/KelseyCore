using UnityEngine;

namespace Kelsey.Extension
{
    public static class CameraExtension
    {
        public static float GetWidthSize(this Camera camera)
        {
            float height = 2f * camera.orthographicSize;
            float width = height * camera.aspect;
            return width;
        }

        public static float GetHeightSize(this Camera camera)
        {
            float height = 2f * camera.orthographicSize;
            return height;
        }

        public static float GetWidthPositionConvertedFromRange(this Camera camera, float origin)
        {
            return GetWidthSize(camera) * origin / 2f;
        }

        public static bool IsObjectVisible(this UnityEngine.Camera @this, UnityEngine.Renderer renderer)
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
        }
    }
}