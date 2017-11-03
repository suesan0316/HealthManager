using System;
using Android.App;
using Android.Content;
using Android.Provider;
using HealthManager.DependencyInterface;
using HealthManager.Droid.DependencyImplement;
using Java.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraDependencyServiceImplementation))]

namespace HealthManager.Droid.DependencyImplement
{
    public class CameraDependencyServiceImplementation : ICameraDependencyService
    {

        public void BringUpCamera()
        {
            File file = new File("image.jpg");
            var intent = new Intent(MediaStore.ActionImageCapture);
            ((Activity)Forms.Context).StartActivityForResult(intent, 1);
        }

        public void BringUpPhotoGallery()
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);

            ((Activity)Forms.Context).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }
    }
}