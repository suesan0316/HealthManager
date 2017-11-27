using System;
using System.Collections.Generic;
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
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     ホーム画面のVMクラス
    /// </summary>
    public class HomeViewModel : INotifyPropertyChanged
    {
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
        ///     身長
        /// </summary>
        private float _height;

        /// <summary>
        ///     読み込み中フラグ
        /// </summary>
        private bool _isLoading;

        /// <summary>
        ///     ニュース一覧(キーにニュースタイトル、値にURL)
        /// </summary>
        public Dictionary<string, string> ItemsDictionary;

        public HomeViewModel()
        {
            MoveToRegistBodyImageCommand = new Command(MoveToRegistBodyImage);
            MoveToRegistBasicDataCommand = new Command(MoveToRegistBasicData);
            MoveToBodyFatPercentageOfDataChartCommand = new Command(MoveToBodyFatPercentageOfDataChart);
            MoveToBodyImageListCommand = new Command(MoveToBodyImageList);
            MoveToBodyWeightOfDataChartCommand = new Command(MoveToViewBodyWeightOfDataChart);
            NewsListItemTappedCommand = new Command<string>(item =>
            {
                ((App) Application.Current).ChangeScreen(new NewsWebView(ItemsDictionary[item]));
            });

            var bodyImageModel = BodyImageService.GetBodyImage();
            if (bodyImageModel != null)
            {
                var imageAsBytes = Convert.FromBase64String(bodyImageModel.ImageBase64String);
                BodyImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                BodyImageRegistedDateString =
                    LanguageUtils.Get(LanguageKeys.RegistedDate) +
                    ViewModelCommonUtil.FormatDateString(bodyImageModel.RegistedDate);
            }
            else
            {
                // 登録されている体格画像がない場合はイメージなし用の画像を表示する
                var imageAsBytes = Convert.FromBase64String(ViewModelConst.NoImageString64);
                BodyImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                BodyImageRegistedDateString =
                    LanguageUtils.Get(LanguageKeys.RegistedDate) + StringConst.Empty;
            }

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
            }
            // ニュース一覧を取得
            Task.Run(SetNewsSourceTask);
        }

        /// <summary>
        ///     ニュース一覧のアイテムリスト
        /// </summary>
        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

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
        ///     体重遷移ボタンコマンド
        /// </summary>
        public ICommand MoveToBodyWeightOfDataChartCommand { get; set; }

        /// <summary>
        ///     体脂肪率遷移ボタンコマンド
        /// </summary>
        public ICommand MoveToBodyFatPercentageOfDataChartCommand { get; set; }

        /// <summary>
        ///     体格遷移ボタンコマンド
        /// </summary>
        public ICommand MoveToBodyImageListCommand { get; set; }

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
        public string Name { get; set; }

        /// <summary>
        ///     名前ラベル
        /// </summary>
        public string NameLabel => LanguageUtils.Get(LanguageKeys.Name) + StringConst.Colon;

        /// <summary>
        ///     性別
        /// </summary>
        public string Gender { get; }

        /// <summary>
        ///     性別ラベル
        /// </summary>
        public string GenderLabel => LanguageUtils.Get(LanguageKeys.Gender) + StringConst.Colon;

        /// <summary>
        ///     年齢
        /// </summary>
        public int Age { get; set; }

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
        public float BodyFatPercentage { get; set; }

        /// <summary>
        ///     体脂肪率ラベル
        /// </summary>
        public string BodyFatPercentageLabel => LanguageUtils.Get(LanguageKeys.BodyFatPercentage) + StringConst.Colon;

        /// <summary>
        ///     上の血圧
        /// </summary>
        public int MaxBloodPressure { get; set; }

        /// <summary>
        ///     下の血圧
        /// </summary>
        public int MinBloodPressure { get; set; }

        /// <summary>
        ///     血圧ラベル
        /// </summary>
        public string BloodPressureLabel => LanguageUtils.Get(LanguageKeys.BloodPressure) + StringConst.Colon;

        /// <summary>
        ///     基礎代謝
        /// </summary>
        public int BasalMetabolism { get; set; }

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
        ///     体重遷移ボタンラベル
        /// </summary>
        public string MoveToBodyWeightOfDataChartLabel => LanguageUtils.Get(LanguageKeys.BodyWeightTransition);

        /// <summary>
        ///     体脂肪率遷移ボタン
        /// </summary>
        public string MoveToBodyFatPercentageOfDataChartLabel =>
            LanguageUtils.Get(LanguageKeys.BodyFatPercentageTransition);

        /// <summary>
        ///     基本データ更新ボタンラベル
        /// </summary>
        public string MoveToRegistBasicDataLabel => LanguageUtils.Get(LanguageKeys.UpdateBasicData);

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
            ItemsDictionary = await service.GetNewsDictionary();
            ItemsDictionary.ForEach(data => Items.Add(data.Key));
            IsLoading = false;
        }

        /// <summary>
        ///     体格登録画面遷移
        /// </summary>
        private static void MoveToRegistBodyImage()
        {
            ((App) Application.Current).ChangeScreen(new RegistBodyImageView());
        }

        /// <summary>
        ///     基本データ登録画面遷移
        /// </summary>
        private static void MoveToRegistBasicData()
        {
            ((App) Application.Current).ChangeScreen(new InputBasicDataView());
        }

        /// <summary>
        ///     データチャート画面遷移(体重)
        /// </summary>
        public void MoveToViewBodyWeightOfDataChart()
        {
            ((App) Application.Current).ChangeScreen(new DataChartView(BasicDataEnum.BodyWeight));
        }

        /// <summary>
        ///     データチャート画面遷移(体脂肪率)
        /// </summary>
        private static void MoveToBodyFatPercentageOfDataChart()
        {
            ((App) Application.Current).ChangeScreen(new DataChartView(BasicDataEnum.BodyFatPercentage));
        }

        /// <summary>
        ///     体格遷移画面遷移
        /// </summary>
        private static void MoveToBodyImageList()
        {
            ((App) Application.Current).ChangeScreen(new BodyImageView());
        }
    }
}