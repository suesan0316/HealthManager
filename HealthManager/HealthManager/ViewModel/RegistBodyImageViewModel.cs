using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Language;
using HealthManager.DependencyInterface;
using HealthManager.Model.Service.Interface;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// 体格画像登録画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class RegistBodyImageViewModel
    {
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Class Variable
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Class Variable

        /// <summary>
        /// 画像ファイル格納フォルダ
        /// </summary>
        private const string BodyImageFileDirectory = "BodyImage";

        /// <summary>
        /// 画像ファイル名ヘッダ
        /// </summary>
        private const string BodyImageNameHead = "bodyImage_";

        /// <summary>
        /// 画像ファイル拡張子
        /// </summary>
        private const string BodyImageExtension = ".jpg";

        #endregion Class Variable
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Instance Private Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Instance Private Variables
            
        private string _filePath;
        private bool _takePhotoFromCamera;
        private readonly int _id;
        private string _base64String;
        private readonly IBodyImageService _bodyImageService;

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public RegistBodyImageViewModel(IBodyImageService bodyImageService)
        {
            _bodyImageService = bodyImageService;
            InitCommands();
            var model = _bodyImageService.GetBodyImage();
            _id = model?.Id ?? 0;
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        /// <summary>
        /// カメラで撮影またはライブラリから選択された画像
        /// </summary>
        public ImageSource BodyImage { get; set; }

        /// <summary>
        /// 登録ボタン表示フラグ
        /// </summary>
        public bool RegistButtonIsVisible => BodyImage != null;

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelCancel => LanguageUtils.Get(LanguageKeys.Cancel);
        public string DisplayLabelRegist => LanguageUtils.Get(LanguageKeys.Regist);
        public string DisplayLabelTakeCamera => LanguageUtils.Get(LanguageKeys.SnapFromCamera);
        public string DisplayLabelTakeLibrary => LanguageUtils.Get(LanguageKeys.SelectFromLibrary);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        public ICommand CommandCancel { get; set; }
        public ICommand CommandRegist { get; set; }
        public ICommand CommandTakeCamera { get; set; }
        public ICommand CommandTakeLibrary { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        /// <summary>
        /// カメラ撮影アクション
        /// </summary>
        /// <returns></returns>
        private async Task CommandTakeCameraAction()
        {
            _takePhotoFromCamera = true;

            try
            {
                // カメラが有効であるか判定
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Error),
                        LanguageUtils.Get(LanguageKeys.CameraNotAvailable), LanguageUtils.Get(LanguageKeys.OK));
                    return;
                }

                // 画像ファイルの名前を決定
                var fileName = BodyImageNameHead + ViewModelCommonUtil.FormatDateStringWithoutSymbol(DateTime.Now) +
                               BodyImageExtension;
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    Directory = BodyImageFileDirectory,
                    Name = fileName,
                    SaveToAlbum = false
                });

                if (file == null)
                    return;

                _filePath = file.Path;

                BodyImage = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

                var imgData = ViewModelCommonUtil.ConvertToByteArrayFromStream(file.GetStream());
                _base64String = Convert.ToBase64String(imgData);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// フォトライブラリ選択アクション
        /// </summary>
        /// <returns></returns>
        private async Task CommandTakeLibraryAction()
        {
            _takePhotoFromCamera = false;

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Error),
                    LanguageUtils.Get(LanguageKeys.PictureLibraryCanNotOpen), LanguageUtils.Get(LanguageKeys.OK));
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
            });

            if (file == null)
                return;

            BodyImage = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            var imgData = ViewModelCommonUtil.ConvertToByteArrayFromStream(file.GetStream());
            _base64String = Convert.ToBase64String(imgData);
        }

        private async Task CommandRegistAction()
        {
            if (_bodyImageService.CheckExitTargetDayData(DateTime.Now))
            {
                var result =
                    await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                        LanguageUtils.Get(LanguageKeys.TodayDataUpdateConfirm), LanguageUtils.Get(LanguageKeys.OK),
                        LanguageUtils.Get(LanguageKeys.Cancel));
                if (result)
                    _bodyImageService.UpdateBodyImage(_id, _base64String);
            }
            else
            {
                _bodyImageService.RegistBodyImage(_base64String);
            }

            if (_takePhotoFromCamera)
            {
                //TODO 確認のダイアログを表示する
                //TODO ViewModelCommonUtilに移動する？
                DependencyService.Get<IImageService>().DeleteImageFile(_filePath);
            }

            // ホーム画面をリロードする
            ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingHomeReload);
            ViewModelCommonUtil.DataBackPage();
        }

        #endregion Command Actions
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init Commands

        private void InitCommands()
        {
            CommandTakeCamera = new Command(async () => await CommandTakeCameraAction());
            CommandTakeLibrary = new Command(async () => await CommandTakeLibraryAction());
            CommandRegist = new Command(async () => await CommandRegistAction());
            CommandCancel = new Command(ViewModelCommonUtil.DataBackPage);
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

        #endregion ViewModel Logic
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init MessageSubscribe
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init MessageSubscribe

        //private void InitMessageSubscribe()
        //{

        //}

        #endregion Init MessageSubscribe
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Default 
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Default
            
        #endregion Default

    }
}