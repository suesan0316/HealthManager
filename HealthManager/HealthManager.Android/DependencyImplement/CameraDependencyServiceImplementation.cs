using System;
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
            var intent = new Intent(MediaStore.ActionImageCapture); 

            MainActivity.File = new Java.IO.File(MainActivity.Dir, $"myPhoto_{Guid.NewGuid()}.jpg");
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(MainActivity.File));
            ((Activity)Forms.Context).StartActivityForResult(intent, 0);
        }
    }
}