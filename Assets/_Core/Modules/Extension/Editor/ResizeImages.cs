using UnityEditor;
using UnityEngine;

namespace Kelsey.ExtensionEditor
{
#if UNITY_EDITOR

    public static class ResizeImages
    {
        private static string basePath => Application.dataPath.Replace("Assets", "");

        [MenuItem("Kelsey/Tools/ResizeImage/Resize Images Divisible by 4")]
        static void ResizeImagess()
        {
            Debug.Log("Start Resize");

            var selecting = Selection.objects;
            foreach (Object o in selecting)
            {
                if (o.GetType() != typeof(Texture2D)) continue;
                Debug.Log("Resize: " + o.name);
                var path = AssetDatabase.GetAssetPath(o);
                var imagePath = basePath + path;
                Texture2D image = LoadImageFromFile(imagePath);

                int newWidth = image.width;
                newWidth = Mathf.RoundToInt(newWidth / 4f) * 4;
                int newHeight = image.height;
                newHeight = Mathf.RoundToInt(newHeight / 4f) * 4;
                Texture2D resizedImage = ResizeImage(image, newWidth, newHeight);

                // Save the resized image to a file
                SaveImageToFile(resizedImage, imagePath);
            }

            Debug.Log("Resize Done!");

            AssetDatabase.Refresh();
        }

        // [MenuItem("Khanh Tool/Resize Bg Vertical 1028")]
        // static void ResizeBgsHorizontal()
        // {
        //     var selecting = Selection.objects;
        //     foreach (Object o in selecting)
        //     {
        //         if (o.GetType() != typeof(Texture2D)) continue;
        //         Debug.Log("Resize: " + o.name);
        //         var path = AssetDatabase.GetAssetPath(o);
        //         var imagePath = basePath + path;
        //         Texture2D image = LoadImageFromFile(imagePath);
        //
        //         if (image.height > 2048)
        //         {
        //             var ratio = 2048f / image.height;
        //
        //             int newWidth = image.width;
        //             newWidth = Mathf.RoundToInt(newWidth * ratio);
        //             int newHeight = image.height;
        //             newHeight = Mathf.RoundToInt(newHeight * ratio);
        //             Texture2D resizedImage = ResizeImage(image, newWidth, newHeight);
        //
        //             // Save the resized image to a file
        //             SaveImageToFile(resizedImage, imagePath);
        //         }
        //     }
        //
        //     ResizeImagess();
        // }

        [MenuItem("Kelsey/Tools/ResizeImage/Resize Bg Vertical 2048")]
        static void ResizeBgsHorizontal()
        {
            var selecting = Selection.objects;
            foreach (Object o in selecting)
            {
                if (o.GetType() != typeof(Texture2D)) continue;
                Debug.Log("Resize: " + o.name);
                var path = AssetDatabase.GetAssetPath(o);
                var imagePath = basePath + path;
                Texture2D image = LoadImageFromFile(imagePath);

                if (image.height > 2048)
                {
                    var ratio = 2048f / image.height;

                    int newWidth = image.width;
                    newWidth = Mathf.RoundToInt(newWidth * ratio);
                    int newHeight = image.height;
                    newHeight = Mathf.RoundToInt(newHeight * ratio);
                    Texture2D resizedImage = ResizeImage(image, newWidth, newHeight);

                    // Save the resized image to a file
                    SaveImageToFile(resizedImage, imagePath);
                }
            }

            ResizeImagess();
        }

        static Texture2D ResizeImage(Texture2D originalImage, int newWidth, int newHeight)
        {
            // Create a new Texture2D with the desired dimensions
            Texture2D resizedImage = new Texture2D(newWidth, newHeight);

            // Loop through the pixels of the resized image
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // Calculate the coordinates in the original image based on the new dimensions
                    float u = x * 1.0f / newWidth;
                    float v = y * 1.0f / newHeight;
                    Color originalColor = originalImage.GetPixelBilinear(u, v);

                    // Set the color of the pixel in the resized image
                    resizedImage.SetPixel(x, y, originalColor);
                }
            }

            // Apply the changes to the resized image
            resizedImage.Apply();

            return resizedImage;
        }

        // Helper method to load an image from a file
        static Texture2D LoadImageFromFile(string imagePath)
        {
            byte[] imageData = System.IO.File.ReadAllBytes(imagePath);
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(imageData);
            return image;
        }

        // Helper method to save an image to a file
        static void SaveImageToFile(Texture2D image, string imagePath)
        {
            byte[] imageData = image.EncodeToPNG();
            System.IO.File.WriteAllBytes(imagePath, imageData);
        }
    }
#endif
}