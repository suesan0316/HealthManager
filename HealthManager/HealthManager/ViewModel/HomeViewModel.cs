using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Extention;
using HealthManager.Logic.News.Factory;
using HealthManager.Model.Service;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     ホーム画面のVMクラス
    /// </summary>
    public class HomeViewModel : INotifyPropertyChanged
    {
        private ImageSource _bodyImage;
        private string _bodyImageRegistedDateString;
        private float _bodyWeight;

        private float _height;

        private bool _isLoading;

        public Dictionary<string, string> ItemsDictionary;

        public HomeViewModel()
        {
            MoveToRegistBodyImageCommand = new Command(MoveToRegistBodyImage);
            MoveToRegistBasicDataCommand = new Command(MoveToRegistBasicData);
            MoveToBodyFatPercentageOfDataChartCommand = new Command(MoveToBodyFatPercentageOfDataChart);
            MoveToBodyImageListCommand = new Command(MoveToBodyImageList);
            MoveToBodyWeightOfDataChartCommand = new Command(MoveToViewBodyWeightOfDataChart);
            ItemTappedCommand = new Command<string>(item =>
            {
                ((App) Application.Current).ChangeScreen(new NewsWebView(ItemsDictionary[item]));
            });

            var bodyImageModel = BodyImageService.GetBodyImage();
            if (bodyImageModel != null)
            {
                var imageAsBytes = Convert.FromBase64String(bodyImageModel.ImageBase64String);
                BodyImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                BodyImageRegistedDateString =
                    "登録日 : " + ViewModelCommonUtil.FormatDateString(bodyImageModel.RegistedDate);
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

            Task.Run(SetNewsSourceTask);
        }

        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

        public ICommand ItemTappedCommand { get; set; }
        public ICommand MoveToRegistBodyImageCommand { get; set; }
        public ICommand MoveToRegistBasicDataCommand { get; set; }
        public ICommand MoveToBodyWeightOfDataChartCommand { get; set; }
        public ICommand MoveToBodyFatPercentageOfDataChartCommand { get; set; }
        public ICommand MoveToBodyImageListCommand { get; set; }

        public ImageSource BodyImage
        {
            get => _bodyImage;
            set
            {
                _bodyImage = value;
                OnPropertyChanged(nameof(BodyImage));
            }
        }

        public string BodyImageRegistedDateString
        {
            get => _bodyImageRegistedDateString;
            set
            {
                _bodyImageRegistedDateString = value;
                OnPropertyChanged(nameof(BodyImageRegistedDateString));
            }
        }

        public string Name { get; set; }

        public string Gender { get; }

        public int Age { get; set; }

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
                    return "0";
                }
            }
        }

        public float BodyFatPercentage { get; set; }

        public int MaxBloodPressure { get; set; }

        public int MinBloodPressure { get; set; }

        public int BasalMetabolism { get; set; }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
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
            ItemsDictionary = await service.GetNewsDictionary();
            ItemsDictionary.ForEach(data => Items.Add(data.Key));
            IsLoading = false;
        }

        private static void MoveToRegistBodyImage()
        {
            ((App) Application.Current).ChangeScreen(new RegistBodyImageView());
        }

        private static void MoveToRegistBasicData()
        {
            ((App) Application.Current).ChangeScreen(new InputBasicDataView());
        }

        public void MoveToViewBodyWeightOfDataChart()
        {
            ((App) Application.Current).ChangeScreen(new DataChartView(BasicDataEnum.BodyWeight));
        }

        private static void MoveToBodyFatPercentageOfDataChart()
        {
            ((App) Application.Current).ChangeScreen(new DataChartView(BasicDataEnum.BodyFatPercentage));
        }

        private static void MoveToBodyImageList()
        {
            ((App) Application.Current).ChangeScreen(new BodyImageView());
        }
    }
}