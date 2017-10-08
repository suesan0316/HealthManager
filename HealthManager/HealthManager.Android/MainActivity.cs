using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using HealthManager.Droid.DependencyImplement;
using Java.IO;

namespace HealthManager.Droid
{
    [Activity(Label = "HealthManager", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        public static Java.IO.File _file;
        public static Java.IO.File _dir;

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
            _dir = new Java.IO.File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


            /*Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);     // ①
            var contentUri = Android.Net.Uri.FromFile(MainActivity._file);                           // ②
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);*/


            //Since we set the request code to 1 for both the camera and photo gallery, that's what we need to check for
            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.Data != null)
                    {
                        //Grab the Uri which is holding the path to the image
                        Android.Net.Uri uri = data.Data;

                        //Read the meta data of the image to determine what orientation the image should be in
                        int orientation = getOrientation(uri);

                        //Stat a background task so we can do all the bitmap stuff off the UI thread
                        BitmapWorkerTask task = new BitmapWorkerTask(this.ContentResolver, uri);
                        task.Execute(orientation);
                    }
                }
            }
            else if(requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {

                    Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);     // ①
                    var contentUri = Android.Net.Uri.FromFile(_file);                           // ②
                    mediaScanIntent.SetData(contentUri);
                    SendBroadcast(mediaScanIntent);

                    // Display in ImageView. We will resize the bitmap to fit the display.
                    // Loading the full sized image will consume to much memory
                    // and cause the application to crash.

                    int height = Resources.DisplayMetrics.HeightPixels;
                    int width = 500;
                    var bitmap = _file.Path.LoadAndResizeBitmap(width, height);              // ③
                   /* if (bitmap != null)
                    {
                        image1.SetImageBitmap(bitmap);                                          // ④
                        bitmap = null;
                    }*/
                    // Dispose of the Java side bitmap.

                    BitmapWorkerTask task = new BitmapWorkerTask(bitmap);
                    task.Execute();

                    GC.Collect();

                    //Bitmap bit = BitmapFactory.DecodeFile(MainActivity._file.Path);



                    /*int height = Resources.DisplayMetrics.HeightPixels;
                    int width = image1.Width;
                    var bitmap = _file.Path.LoadAndResizeBitmap(width, height);              // ③
                    if (bitmap != null)
                    {
                        image1.SetImageBitmap(bitmap);e                                          // ④
                        bitmap = null;
                    }*/
                    // Dispose of the Java side bitmap.
                    //GC.Collect();

                    //Read the meta data of the image to determine what orientation the image should be in
                    //int orientation = getOrientation(contentUri);

                    //Stat a background task so we can do all the bitmap stuff off the UI thread
                    
                }
            }
        }

        public int getOrientation(Android.Net.Uri photoUri)
        {
            ICursor cursor = Application.ApplicationContext.ContentResolver.Query(photoUri, new String[] { MediaStore.Images.ImageColumns.Orientation }, null, null, null);

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
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                    ? outHeight / height
                    : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }
}

