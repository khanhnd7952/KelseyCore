using UnityEngine;

namespace Kelsey.Extension
{
    /// <summary>
    /// Small helper class to convert viewport, screen or world positions to canvas space.
    /// Only works with screen space canvases.
    /// </summary>
    /// <example>
    /// <code>
    /// objectOnCanvasRectTransform.anchoredPosition = specificCanvas.WorldToCanvasPoint(worldspaceTransform.position);
    /// </code>
    /// </example>
    public static class CanvasPositioningExtensions
    {
        public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }

            var viewportPosition = camera.WorldToViewportPoint(worldPosition);
            return canvas.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
        {
            var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                screenPosition.y / Screen.height,
                0);
            return canvas.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
        {
            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
            var canvasRect = canvas.GetComponent<RectTransform>();
            var scale = canvasRect.sizeDelta;
            return Vector3.Scale(centerBasedViewPortPosition, scale);
        }

        public static Vector3 CanvasObjectToWorldPosition(this Transform canvasObject, Canvas canvas,
            Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            
            if(camera == null) return Vector3.zero;

            var canvasPosition = canvas.transform.InverseTransformPoint(canvasObject.position);
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 viewportPosition = new Vector2(
                (canvasPosition.x / canvasRect.sizeDelta.x) + 0.5f,
                (canvasPosition.y / canvasRect.sizeDelta.y) + 0.5f
            );

            Vector3 worldPosition =
                camera.ViewportToWorldPoint(new Vector3(viewportPosition.x, viewportPosition.y, camera.nearClipPlane));
            return worldPosition;
        }

        public static Vector2 CanvasObjectToWorldSize(this RectTransform canvasObject, Canvas canvas,
            Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            
            if(camera == null) return Vector2.zero;

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            Vector2 viewportSize = new Vector2(
                canvasObject.rect.width / canvasRect.sizeDelta.x,
                canvasObject.rect.height / canvasRect.sizeDelta.y
            );

            Vector3 worldSizeMin = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            Vector3 worldSizeMax = camera.ViewportToWorldPoint(new Vector3(viewportSize.x, viewportSize.y, camera.nearClipPlane));
            Vector2 worldSize = new Vector2(
                Mathf.Abs(worldSizeMax.x - worldSizeMin.x),
                Mathf.Abs(worldSizeMax.y - worldSizeMin.y)
            );

            return worldSize;
        }
    }
}