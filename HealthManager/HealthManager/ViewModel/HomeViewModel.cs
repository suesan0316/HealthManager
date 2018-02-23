using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Properties;
using HealthManager.View;
using HealthManager.ViewModel.Logic.News.Factory;
using HealthManager.ViewModel.Structure;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     ホーム画面のVMクラス
    /// </summary>
    public class HomeViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     年齢
        /// </summary>
        private int _age;

        /// <summary>
        ///     基礎代謝
        /// </summary>
        private int _basalMetabolism;

        /// <summary>
        ///     体脂肪率
        /// </summary>
        private float _bodyFatPercentage;

        /// <summary>
        ///     表示体格画像
        /// </summary>
        private ImageSource _bodyImage;

        /// <summary>
        ///     体格画像登録日
        /// </summary>
        private string _bodyImageRegistedDateString;

        /// <summary>
        ///     体重
        /// </summary>
        private float _bodyWeight;

        /// <summary>
        ///     性別
        /// </summary>
        private string _gender;

        /// <summary>
        ///     身長
        /// </summary>
        private float _height;

        /// <summary>
        ///     読み込み中フラグ
        /// </summary>
        private bool _isLoading;

        /// <summary>
        ///     上の血圧
        /// </summary>
        private int _maxBloodPressure;

        /// <summary>
        ///     下の血圧
        /// </summary>
        private int _minBloodPressure;

        private string _moveTioRegistBasicDataImageSource;

        /// <summary>
        ///     名前
        /// </summary>
        private string _name;

        public HomeViewModel()
        {
            // 基本情報入力画面から戻ってきた際に基本情報をリロードする
            MessagingCenter.Subscribe<InputBasicDataViewModel>(this, ViewModelConst.MessagingHomeReload,
                (sender) => { ReloadBasicData(); });

            // 体格画像登録画面から戻ってきた際に画像をリロードする
            MessagingCenter.Subscribe<RegistBodyImageViewModel>(this, ViewModelConst.MessagingHomeReload,
                (sender) => { ReloadImage(); });

            // 各コマンドの初期化
            MoveToRegistBodyImageCommand = new Command(MoveToRegistBodyImage);
            MoveToRegistBasicDataCommand = new Command(MoveToRegistBasicData);
            MoveToDataChartCommand = new Command(MoveToDataChart);
            NewsListItemTappedCommand = new Command<NewsStructure>(item =>
            {
                ViewModelConst.DataPageNavigation.PushAsync(new NewsWebView(item.NewsUrl,ViewModelConst.DataPageNavigation));
            });

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

                MoveToBodyImageListCommand = new Command(MoveToBodyImageList);
            }
            else
            {
                // 登録されている体格画像がない場合はイメージなし用の画像を表示する
                var imageAsBytes = Convert.FromBase64String(ViewModelConst.NoImageString64);
                BodyImage = ImageSource.FromStream(() =>
                    new MemoryStream(ViewModelCommonUtil.GetResizeImageBytes(imageAsBytes, 300, 425)));
                BodyImageRegistedDateString =
                    LanguageUtils.Get(LanguageKeys.RegistedDate) + StringConst.Empty;

                MoveToBodyImageListCommand = new Command(MoveToRegistBodyImage);
            }

            // 基本データを取得
            var model = BasicDataService.GetBasicData();
            if (model != null)
            {
                Name = model.Name;
                Gender = ((GenderEnum) model.Gender).ToString();
                Age = model.Age;
                Height = model.Height;
                BodyWeight = model.BodyWeight;
                BodyFatPercentage = model.BodyFatPercentage;
                MaxBloodPressure = model.MaxBloodPressure;
                MinBloodPressure = model.MinBloodPressure;
                BasalMetabolism = model.BasalMetabolism;
                switch (model.Gender)
                {
                    case (int) GenderEnum.男性:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.ManImage;
                        break;
                    case (int) GenderEnum.女性:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.WomanImage;
                        break;
                    default:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.PersonImage;
                        break;
                }
            }

            // ニュース一覧を取得
            Task.Run(SetNewsSourceTask);
        }

        /// <summary>
        ///     ニュース一覧のアイテムリスト
        /// </summary>
        public ObservableCollection<NewsStructure> Items { protected set; get; } =
            new ObservableCollection<NewsStructure>();

        /// <summary>
        ///     ニュース一覧アイテムタップコマンド
        /// </summary>
        public ICommand NewsListItemTappedCommand { get; set; }

        /// <summary>
        ///     体格画像登録ボタンコマンド
        /// </summary>
        public ICommand MoveToRegistBodyImageCommand { get; set; }

        /// <summary>
        ///     基本データ更新ボタンコマンド
        /// </summary>
        public ICommand MoveToRegistBasicDataCommand { get; set; }

        /// <summary>
        ///     体格遷移ボタンコマンド
        /// </summary>
        public ICommand MoveToBodyImageListCommand { get; set; }

        /// <summary>
        ///     データチャート遷移ボタンコマンド
        /// </summary>
        public ICommand MoveToDataChartCommand { get; set; }

        /// <summary>
        ///     体格画像
        /// </summary>
        public ImageSource BodyImage
        {
            get => _bodyImage;
            set
            {
                _bodyImage = value;
                OnPropertyChanged(nameof(BodyImage));
            }
        }

        /// <summary>
        ///     体格画像登録日
        /// </summary>
        public string BodyImageRegistedDateString
        {
            get => _bodyImageRegistedDateString;
            set
            {
                _bodyImageRegistedDateString = value;
                OnPropertyChanged(nameof(BodyImageRegistedDateString));
            }
        }

        /// <summary>
        ///     名前
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        /// <summary>
        ///     名前ラベル
        /// </summary>
        public string NameLabel => LanguageUtils.Get(LanguageKeys.Name) + StringConst.Colon;

        /// <summary>
        ///     性別
        /// </summary>
        public string Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        /// <summary>
        ///     性別ラベル
        /// </summary>
        public string GenderLabel => LanguageUtils.Get(LanguageKeys.Gender) + StringConst.Colon;

        /// <summary>
        ///     年齢
        /// </summary>
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        /// <summary>
        ///     年齢ラベル
        /// </summary>
        public string AgeLabel => LanguageUtils.Get(LanguageKeys.Age) + StringConst.Colon;

        /// <summary>
        ///     身長
        /// </summary>
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Bmi));
            }
        }

        /// <summary>
        ///     身長ラベル
        /// </summary>
        public string HeightLabel => LanguageUtils.Get(LanguageKeys.Height) + StringConst.Colon;

        /// <summary>
        ///     体重
        /// </summary>
        public float BodyWeight
        {
            get => _bodyWeight;
            set
            {
                _bodyWeight = value;
                OnPropertyChanged(nameof(BodyWeight));
                OnPropertyChanged(nameof(Bmi));
            }
        }

        /// <summary>
        ///     体重ラベル
        /// </summary>
        public string BodyWeightLabel => LanguageUtils.Get(LanguageKeys.BodyWeight) + StringConst.Colon;

        /// <summary>
        ///     BMI(自動計算)
        /// </summary>
        public string Bmi
        {
            get
            {
                try
                {
                    var tmp = _bodyWeight / Math.Pow(_height / 100f, 2);
                    return CommonUtil.GetDecimalFormatString(double.IsNaN(tmp) ? 0 : tmp);
                }
                catch (Exception)
                {
                    return StringConst.Zero;
                }
            }
        }

        /// <summary>
        ///     BMIラベル
        /// </summary>
        public string BmiLabel => LanguageUtils.Get(LanguageKeys.BMI) + StringConst.Colon;

        /// <summary>
        ///     体脂肪率
        /// </summary>
        public float BodyFatPercentage
        {
            get => _bodyFatPercentage;
            set
            {
                _bodyFatPercentage = value;
                OnPropertyChanged(nameof(BodyFatPercentage));
            }
        }

        /// <summary>
        ///     体脂肪率ラベル
        /// </summary>
        public string BodyFatPercentageLabel => LanguageUtils.Get(LanguageKeys.BodyFatPercentage) + StringConst.Colon;

        /// <summary>
        ///     上の血圧
        /// </summary>
        public int MaxBloodPressure
        {
            get => _maxBloodPressure;
            set
            {
                _maxBloodPressure = value;
                OnPropertyChanged(nameof(MaxBloodPressure));
            }
        }

        /// <summary>
        ///     下の血圧
        /// </summary>
        public int MinBloodPressure
        {
            get => _minBloodPressure;
            set
            {
                _minBloodPressure = value;
                OnPropertyChanged(nameof(MinBloodPressure));
            }
        }

        /// <summary>
        ///     血圧ラベル
        /// </summary>
        public string BloodPressureLabel => LanguageUtils.Get(LanguageKeys.BloodPressure) + StringConst.Colon;

        /// <summary>
        ///     基礎代謝
        /// </summary>
        public int BasalMetabolism
        {
            get => _basalMetabolism;
            set
            {
                _basalMetabolism = value;
                OnPropertyChanged(nameof(BasalMetabolism));
            }
        }

        /// <summary>
        ///     基礎代謝ラベル
        /// </summary>
        public string BasalMetabolismLabel => LanguageUtils.Get(LanguageKeys.BasicMetabolism) + StringConst.Colon;

        /// <summary>
        ///     読み込み中フラグ
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        /// <summary>
        ///     体格画像登録ボタンラベル
        /// </summary>
        public string MoveToRegistBodyImageLabel => LanguageUtils.Get(LanguageKeys.RegistBodyImage);

        /// <summary>
        ///     体格画像遷移ボタンラベル
        /// </summary>
        public string MoveToBodyImageListLabel => LanguageUtils.Get(LanguageKeys.WatchBodyTransition);

        /// <summary>
        ///     データチャート遷移ボタンラベル
        /// </summary>
        public string MoveToDataChartLabel => LanguageUtils.Get(LanguageKeys.DataChart);

        /// <summary>
        ///     基本データ更新ボタンラベル
        /// </summary>
        public string MoveToRegistBasicDataLabel => LanguageUtils.Get(LanguageKeys.UpdateBasicData);

        /// <summary>
        ///     ニュース一覧タイトルラベル
        /// </summary>
        public string NewsListTitleLabel => LanguageUtils.Get(LanguageKeys.NewsListTitle);

        /// <summary>
        ///     基本データ更新ボタンファイルソース
        /// </summary>
        public string MoveTioRegistBasicDataImageSource
        {
            get => _moveTioRegistBasicDataImageSource;
            set
            {
                _moveTioRegistBasicDataImageSource = value;
                OnPropertyChanged(nameof(MoveTioRegistBasicDataImageSource));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     ニュース一覧を取得
        /// </summary>
        /// <returns></returns>
        private async Task SetNewsSourceTask()
        {
            IsLoading = true;
            var service = NewsServiceFactory.CreateNewsService();
            var structures = await service.GetHealthNewsData();
            structures.ForEach(data => Items.Add(data));
            IsLoading = false;
        }

        /// <summary>
        ///     体格登録画面遷移
        /// </summary>
        private static void MoveToRegistBodyImage()
        {
            ViewModelConst.DataPageNavigation.PushAsync(new RegistBodyImageView());
        }

        /// <summary>
        ///     基本データ登録画面遷移
        /// </summary>
        private void MoveToRegistBasicData()
        {
            ViewModelConst.DataPageNavigation.PushAsync(new InputBasicDataView());
        }

        /// <summary>
        ///     データチャート画面遷移
        /// </summary>
        public void MoveToDataChart()
        {
            //((App)Application.Current).ChangeScreen(new DataSelectView());
            ViewModelConst.DataPageNavigation.PushAsync(new DataSelectView());
        }

        /// <summary>
        ///     体格遷移画面遷移
        /// </summary>
        public void MoveToBodyImageList()
        {
            ViewModelConst.DataPageNavigation.PushAsync(new BodyImageView());
        }

        /// <summary>
        ///     基本データのリロード
        /// </summary>
        public void ReloadBasicData()
        {
            // 基本データを取得
            var model = BasicDataService.GetBasicData();
            if (model != null)
            {
                Name = model.Name;
                Gender = ((GenderEnum) model.Gender).ToString();
                Age = model.Age;
                Height = model.Height;
                BodyWeight = model.BodyWeight;
                BodyFatPercentage = model.BodyFatPercentage;
                MaxBloodPressure = model.MaxBloodPressure;
                MinBloodPressure = model.MinBloodPressure;
                BasalMetabolism = model.BasalMetabolism;
                switch (model.Gender)
                {
                    case (int) GenderEnum.男性:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.ManImage;
                        break;
                    case (int) GenderEnum.女性:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.WomanImage;
                        break;
                    default:
                        MoveTioRegistBasicDataImageSource = ViewModelConst.PersonImage;
                        break;
                }
            }
        }

        /// <summary>
        ///     体格画像のリロード
        /// </summary>
        public void ReloadImage()
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

                MoveToBodyImageListCommand = new Command(MoveToBodyImageList);
            }
            else
            {
                // 登録されている体格画像がない場合はイメージなし用の画像を表示する
                var imageAsBytes = Convert.FromBase64String(ViewModelConst.NoImageString64);
                BodyImage = ImageSource.FromStream(() =>
                    new MemoryStream(ViewModelCommonUtil.GetResizeImageBytes(imageAsBytes, 300, 425)));
                BodyImageRegistedDateString =
                    LanguageUtils.Get(LanguageKeys.RegistedDate) + StringConst.Empty;

                MoveToBodyImageListCommand = new Command(MoveToRegistBodyImage);
            }
        }
    }
}