using System;
using System.ComponentModel;
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
	    private static readonly string BodyImageFileDirectory = "BodyImage";
	    private static readonly string BodyImageNameHead = "bodyImage_";
	    private static readonly string BodyImageExtension = ".jpg";

		private string _base64String;

        private ImageSource _bodyImage;
        private readonly int _id;
        private bool _takePhotoFromCamera = false;

        public RegistBodyImageViewModel()
        {
            TakeImageCameraCommand = new Command(async () => await TakeImageCamera());
            TakeImageLibraryaCommand = new Command(async () => await　TakeImageLibrary());
            RegistBodyImageCommand = new Command(async () => await RegistBodyImage());
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);

            var model = BodyImageService.GetBodyImage();
            _id = model?.Id ?? 0;
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

        private async Task TakeImageCamera()
        {
            // TODO カメラから写真を登録した場合は撮った写真は削除する処理を追加する
	        try
	        {
		        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
		        {
			        await Application.Current.MainPage.DisplayAlert("エラー","カメラが有効で張りません", "OK");
			        return;
		        }

		        var fileName = BodyImageNameHead + ViewModelCommonUtil.FormatDateStringWithoutSymbol(DateTime.Now) + BodyImageExtension;
		        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
		        {
			        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
			        Directory = BodyImageFileDirectory,
			        Name = fileName,
					SaveToAlbum = false
		        });

		        if (file == null)
			        return;

		        BodyImage = ImageSource.FromStream(() =>
		        {
			        var stream = file.GetStream();
			        file.Dispose();
			        return stream;
		        });

		        var imgData =ViewModelCommonUtil.ConvertToByteArrayFromStream(file.GetStream());
		        _base64String = Convert.ToBase64String(imgData);

	            _takePhotoFromCamera = true;

	        }
	        catch (Exception e)
	        {
		        System.Diagnostics.Debug.WriteLine(e.StackTrace);
	        }
        }

		private async Task TakeImageLibrary()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("エラー", "写真ライブラリを開けません", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });


            if (file == null)
                return;

            BodyImage　= ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            var imgData = ViewModelCommonUtil.ConvertToByteArrayFromStream(file.GetStream());
            _base64String = Convert.ToBase64String(imgData);

            _takePhotoFromCamera = false;
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