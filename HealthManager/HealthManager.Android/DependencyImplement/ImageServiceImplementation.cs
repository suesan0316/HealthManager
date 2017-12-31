using System.IO;
using Android.Graphics;
using HealthManager.DependencyInterface;
using HealthManager.Droid.DependencyImplement;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageServiceImplementation))]

namespace HealthManager.Droid.DependencyImplement
{
    public class ImageServiceImplementation : IImageService
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            var originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            var resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (var ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}