using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using HealthManager.DependencyInterface;
using HealthManager.Droid.DependencyImplement;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraDependencyServiceImplementation))]

namespace HealthManager.Droid.DependencyImplement
{
    public class CameraDependencyServiceImplementation : ICameraDependencyService
    {

        public void BringUpPhotoGallery()
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);

            ((Activity)Forms.Context).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }
        public void BringUpCamera()
        {
            /*var intent = new Intent(MediaStore.ActionImageCapture);
            ((Activity)Forms.Context).StartActivityForResult(intent, 1);*/
            var intent = new Intent(MediaStore.ActionImageCapture);                  // ①

            MainActivity._file = new Java.IO.File(MainActivity._dir, $"myPhoto_{Guid.NewGuid()}.jpg");   // ②
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(MainActivity._file));   // ③
            ((Activity)Forms.Context).StartActivityForResult(intent, 0);
        }
    }
}