using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.View;
using HealthManager.ViewModel.Logic.News.Factory;
using HealthManager.ViewModel.Structure;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     ホーム画面のVMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class HomeViewModel 
    {
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Class Variable
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Class Variable

        #endregion Class Variable
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Instance Private Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Instance Private Variables          

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public HomeViewModel()
        {
            InitMessageSubscribe();
            InitCommands();
            LoadBodyImage();
            LoadBasicData();

            // ニュース一覧を取得
            Task.Run(SetNewsSourceTask);
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        public ObservableCollection<NewsStructure> Items { set; get; } =
            new ObservableCollection<NewsStructure>();
        public ImageSource BodyImage { get; set; }
        public string BodyImageRegistedDateString { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int Age => (int) ((DateTime.Now - Birthday).Days / 365.2425);
        public float Height { get; set; }
        public float BodyWeight { get; set; }
        public string Bmi
        {
            get
            {
                try
                {
                    return ViewModelCommonUtil.GetBmiValueString(height:Height, bodyWeight:BodyWeight);
                }
                catch (Exception)
                {
                    return StringConst.Zero;
                }
            }
        }
        public float BodyFatPercentage { get; set; }
        public int MaxBloodPressure { get; set; }
        public int MinBloodPressure { get; set; }
        public int BasalMetabolism { get; set; }

        // 基本情報の登録が1件もない場合は値を表示しない
        public bool IsVisibleBasicDataValues { get; set; }

        public string MoveTioRegistBasicDataImageSource { get; set; }
        public bool IsLoading { get; set; }

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelAge => LanguageUtils.Get(LanguageKeys.Age) + StringConst.Colon;
        public string DisplayLabelBasalMetabolism => LanguageUtils.Get(LanguageKeys.BasicMetabolism) + StringConst.Colon;
        public string DisplayLabelBloodPressure => LanguageUtils.Get(LanguageKeys.BloodPressure) + StringConst.Colon;
        public string DisplayLabelBmi => LanguageUtils.Get(LanguageKeys.BMI) + StringConst.Colon;
        public string DisplayLabelBodyFatPercentage => LanguageUtils.Get(LanguageKeys.BodyFatPercentage) + StringConst.Colon;
        public string DisplayLabelBodyWeight => LanguageUtils.Get(LanguageKeys.BodyWeight) + StringConst.Colon;
        public string DisplayLabelGender => LanguageUtils.Get(LanguageKeys.Gender) + StringConst.Colon;
        public string DisplayLabelHeight => LanguageUtils.Get(LanguageKeys.Height) + StringConst.Colon;
        public string DisplayLabelBodyImageList => LanguageUtils.Get(LanguageKeys.WatchBodyTransition);
        public string DisplayLabelDataChart => LanguageUtils.Get(LanguageKeys.DataChart);
        public string DisplayLabelBasicData => LanguageUtils.Get(LanguageKeys.UpdateBasicData);
        public string DisplayLabelBodyImage => LanguageUtils.Get(LanguageKeys.RegistBodyImage);
        public string DisplayLabelName => LanguageUtils.Get(LanguageKeys.Name) + StringConst.Colon;
        public string DisplayLabelNewsListTitle => LanguageUtils.Get(LanguageKeys.NewsListTitle);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        public ICommand CommandBasicData { get; set; }
        public ICommand CommandBodyImage { get; set; }
        public ICommand CommandBodyImageList { get; set; }
        public ICommand CommandDataChart { get; set; }
        public ICommand CommandNewsListItemTapped { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        /// <summary>
        /// 体格登録画面遷移
        /// </summary>
        private static void CommandBodyImageAction()
        {
            ViewModelConst.DataPageNavigation.PushAsync(new RegistBodyImageView());
        }

        /// <summary>
        ///  基本データ登録画面遷移
        /// </summary>
        private static void CommandBasicDataAction()
        {
            ViewModelConst.DataPageNavigation.PushAsync(new InputBasicDataView());
        }

        /// <summary>
        ///  データチャート画面遷移
        /// </summary>
        public async  Task CommandDataChartAction()
        {
            var check = BasicDataService.GetBasicData();
            if (check != null)
            {
                await ViewModelConst.DataPageNavigation.PushAsync(new DataSelectView());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.NotExistBasicData), LanguageUtils.Get(LanguageKeys.OK));

                await ViewModelConst.DataPageNavigation.PushAsync(new InputBasicDataView());
            }
        }

        /// <summary>
        ///  体格遷移画面遷移
        /// </summary>
        public async Task CommandBodyImageListAction()
        {
            var check = BodyImageService.GetBodyImageList();
            if (check == null || check.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.NotExistBodyImage), LanguageUtils.Get(LanguageKeys.OK));

                await ViewModelConst.DataPageNavigation.PushAsync(new RegistBodyImageView());
            }
            else
            {
                await ViewModelConst.DataPageNavigation.PushAsync(new BodyImageView());
            }
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
            CommandBodyImage = new Command(CommandBodyImageAction);
            CommandBasicData = new Command(CommandBasicDataAction);
            CommandDataChart = new Command(async() => await CommandDataChartAction());
            CommandBodyImageList = new Command(async () => await CommandBodyImageListAction());
            CommandNewsListItemTapped = new Command<NewsStructure>(item =>
            {
                ViewModelConst.DataPageNavigation.PushAsync(new NewsWebView(item.NewsUrl,
                    ViewModelConst.DataPageNavigation));
            });
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

        /// <summary>
        ///  ニュース一覧を取得
        /// </summary>
        /// <returns></returns>
        private async Task SetNewsSourceTask()
        {
            IsLoading = true;
            var service = NewsServiceFactory.CreateNewsService();
            var structures = await service.GetHealthNewsData();
            Items = new ObservableCollection<NewsStructure>(structures);
            IsLoading = false;
        }

        /// <summary>
        ///  基本データのロード
        /// </summary>
        public void LoadBasicData()
        {
            // 基本データを取得
            var model = BasicDataService.GetBasicData();
            if (model != null)
            {
                Name = model.Name;
                Gender = ((GenderEnum)model.Gender).ToString();
                Birthday = model.BirthDay;
                Height = model.Height;
                BodyWeight = model.BodyWeight;
                BodyFatPercentage = model.BodyFatPercentage;
                MaxBloodPressure = model.MaxBloodPressure;
                MinBloodPressure = model.MinBloodPressure;
                BasalMetabolism = model.BasalMetabolism;
                switch (model.Gender)
                {
                    case (int)GenderEnum.男性:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.ManImage;
                        break;
                    case (int)GenderEnum.女性:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.WomanImage;
                        break;
                    default:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.PersonImage;
                        break;
                }

                IsVisibleBasicDataValues = true;
            }
            else
            {
                MoveTioRegistBasicDataImageSource = ViewModelConst.PersonImage;
                IsVisibleBasicDataValues = false;
            }
        }

        /// <summary>
        ///     体格画像のロード
        /// </summary>
        public void LoadBodyImage()
        {
            // 表示する体格画像を取得
            var bodyImageModel = BodyImageService.GetBodyImage();
            if (bodyImageModel != null)
            {
                var imageAsBytes = Convert.FromBase64String(bodyImageModel.ImageBase64String);

                BodyImage = ImageSource.FromStream(() =>
                    new MemoryStream(ViewModelCommonUtil.GetResizeImageBytes(imageAsBytes, 300, 425)));
                BodyImageRegistedDateString =
                    LanguageUtils.Get(LanguageKeys.RegistedDate) +
                    ViewModelCommonUtil.FormatDateString(bodyImageModel.RegistedDate);
            }
            else
            {
                // 登録されている体格画像がない場合はイメージなし用の画像を表示する
                var imageAsBytes = Convert.FromBase64String(ViewModelConst.NoImageString64);
                BodyImage = ImageSource.FromStream(() =>
                    new MemoryStream(ViewModelCommonUtil.GetResizeImageBytes(imageAsBytes, 300, 425)));
                BodyImageRegistedDateString =
                    LanguageUtils.Get(LanguageKeys.RegistedDate) + StringConst.Empty;
            }
        }

        #endregion ViewModel Logic
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init MessageSubscribe
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init MessageSubscribe

        private void InitMessageSubscribe()
        {
            MessagingCenter.Subscribe<ViewModelCommonUtil>(this, ViewModelConst.MessagingHomeReload,
                sender =>
                {
                    LoadBasicData();
                    LoadBodyImage();
                });
        }

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