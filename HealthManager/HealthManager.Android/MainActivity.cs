using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Provider;
using HealthManager.Droid.DependencyImplement;
using Plugin.Permissions;

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

	    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
	    {
		    PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
        }

        public int getOrientation(Android.Net.Uri photoUri)
        {
            ICursor cursor = Application.ApplicationContext.ContentResolver.Query(photoUri, new[] { MediaStore.Images.ImageColumns.Orientation }, null, null, null);

            if (cursor.Count != 1)
            {
                return -1;
            }

            cursor.MoveToFirst();
            return cursor.GetInt(0);
        }
    }
}

