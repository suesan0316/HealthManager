using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.DependencyInterface;
using HealthManager.Model.Service;
using Plugin.Media;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class RegistBodyImageViewModel : INotifyPropertyChanged
    {
        private string _base64String;

        private ImageSource _bodyImage;
        private readonly int _id;

        public RegistBodyImageViewModel()
        {
            TakeImageCameraCommand = new Command(async () =>  TakeImageCamera());
            TakeImageLibraryaCommand = new Command(TakeImageLibrary);
            RegistBodyImageCommand = new Command(async () => await RegistBodyImage());
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);

            var model = BodyImageService.GetBodyImage();
            _id = model?.Id ?? 0;

            /*MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", args =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _base64String = Convert.ToBase64String(args);
                    var imageAsBytes = Convert.FromBase64String(_base64String);
                    BodyImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                });
            });*/
        }

        public ICommand TakeImageCameraCommand { get; set; }
        public ICommand TakeImageLibraryaCommand { get; set; }
        public ICommand RegistBodyImageCommand { get; set; }
        public ICommand BackHomeCommand { get; set; }

        public ImageSource BodyImage
        {
            get => _bodyImage;
            set
            {
                _bodyImage = value;
                RegistButtonIsVisible = true;
                OnPropertyChanged(nameof(BodyImage));
                OnPropertyChanged(nameof(RegistButtonIsVisible));
            }
        }

        public bool RegistButtonIsVisible { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void TakeImageCamera()
        {
	        try
	        {
		        //DependencyService.Get<ICameraDependencyService>().BringUpCamera();
		        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
		        {
			        await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
			        return;
		        }

		        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
		        {
			        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
			        Directory = "Sample",
			        Name = "test.jpg"
		        });

		        if (file == null)
			        return;

		        //await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

		        BodyImage = ImageSource.FromStream(() =>
		        {
			        var stream = file.GetStream();
			        file.Dispose();
			        return stream;
		        });

		        byte[] imgData = ReadStream(file.GetStream());


				_base64String = Convert.ToBase64String(imgData);


			}
	        catch (Exception e)
	        {
		        System.Diagnostics.Debug.WriteLine(e.StackTrace);
	        }
        }

	    public byte[] ReadStream(Stream input)
	    {
		    byte[] buffer = new byte[16 * 1024];
		    using (MemoryStream ms = new MemoryStream())
		    {
			    int read;
			    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
			    {
				    ms.Write(buffer, 0, read);
			    }
			    return ms.ToArray();
		    }

	    }

		private void TakeImageLibrary()
        {
            DependencyService.Get<ICameraDependencyService>().BringUpPhotoGallery();
        }

        private async Task RegistBodyImage()
        {
            if (BodyImageService.CheckExitTargetDayData(DateTime.Now))
            {
                var result =
                    await Application.Current.MainPage.DisplayAlert("確認", "本日は既にデータを登録しています。更新しますか？", "OK", "キャンセル");
                if (result)
                    BodyImageService.UpdateBodyImage(_id, _base64String);
            }
            else
            {
                BodyImageService.RegistBodyImage(_base64String);
            }
            ViewModelCommonUtil.BackHome();
        }
    }
}