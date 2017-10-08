using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using HealthManager.Droid.DependencyImplement;

namespace HealthManager.Droid
{
    [Activity(Label = "HealthManager", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static Java.IO.File File;
        public static Java.IO.File Dir;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            CreateDirectoryForPictures();

        }

        private void CreateDirectoryForPictures()
        {
            Dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!Dir.Exists())
            {
                Dir.Mkdirs();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.Data != null)
                    {
                        var uri = data.Data;
                        
                        var orientation = GetOrientation(uri);
                        
                        var task = new BitmapWorkerTask(ContentResolver, uri);
                        task.Execute(orientation);
                    }
                }
            }
            else if(requestCode == 0)
            {
                if (resultCode != Result.Ok) return;
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                var contentUri = Android.Net.Uri.FromFile(File); 
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                var height = Resources.DisplayMetrics.HeightPixels;
                var width = 500;
                var bitmap = File.Path.LoadAndResizeBitmap(width, height);

                var task = new BitmapWorkerTask(bitmap);
                task.Execute();

                GC.Collect();
            }
        }

        public int GetOrientation(Android.Net.Uri photoUri)
        {
            var cursor = Application.ApplicationContext.ContentResolver.Query(photoUri, new[] { MediaStore.Images.ImageColumns.Orientation }, null, null, null);

            if (cursor.Count != 1)
            {
                return -1;
            }

            cursor.MoveToFirst();
            return cursor.GetInt(0);
        }
    }
    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            var options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);
            
            var outHeight = options.OutHeight;
            var outWidth = options.OutWidth;
            var inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                    : outWidth / width;
            }
            
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            var resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }
}

