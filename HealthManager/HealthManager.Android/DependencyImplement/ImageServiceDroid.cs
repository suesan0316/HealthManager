using System;
using System.Diagnostics;
using System.IO;
using Android.Graphics;
using HealthManager.DependencyInterface;
using HealthManager.Droid.DependencyImplement;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageServiceDroid))]

namespace HealthManager.Droid.DependencyImplement
{
    public class ImageServiceDroid : IImageService
    {
        public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        {
            if (!File.Exists(targetFile) && File.Exists(sourceFile))
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                var options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                };

                using (var image = BitmapFactory.DecodeFile(sourceFile, options))
                {
                    if (image != null)
                    {
                        var sourceSize = new Size((int) image.GetBitmapInfo().Height,
                            (int) image.GetBitmapInfo().Width);

                        var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                        string targetDir = System.IO.Path.GetDirectoryName(targetFile);
                        if (!Directory.Exists(targetDir))
                            Directory.CreateDirectory(targetDir);

                        if (maxResizeFactor > 0.9)
                        {
                            File.Copy(sourceFile, targetFile);
                        }
                        else
                        {
                            var width = (int) (maxResizeFactor * sourceSize.Width);
                            var height = (int) (maxResizeFactor * sourceSize.Height);

                            using (var bitmapScaled = Bitmap.CreateScaledBitmap(image, height, width, true))
                            {
                                using (Stream outStream = File.Create(targetFile))
                                {
                                    if (targetFile.ToLower().EndsWith("png"))
                                        bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, outStream);
                                    else
                                        bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);
                                }
                                bitmapScaled.Recycle();
                            }
                        }

                        image.Recycle();
                    }
                }
            }
        }
    }
}