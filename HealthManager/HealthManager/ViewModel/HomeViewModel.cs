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
    public class HomeViewModel : INotifyPropertyChanged
    {
        private ImageSource _bodyImage;


        private string _bodyImageRegistedDateString;

        private float _bodyWeight;

        private float _height;

        // 読み込み中フラグ
        private bool _isLoading;

        public Dictionary<string, string> ItemsDictionary;

        public HomeViewModel()
        {
            RegistBodyImageCommand = new Command(RegistBodyImage);
            RegistBasicDataCommand = new Command(RegistBasicData);
            ViewBodyFatPercentageChartCommand = new Command(ViewBodyFatPercentageChart);
            ViewBodyImageListCommand = new Command(ViewBodyImageList);
            ViewBodyWeightChartCommand = new Command(ViewBodyWeightChart);

            ItemTappedCommand = new Command<string>(item =>
            {
                ((App) Application.Current).ChangeScreen(new NewsWebView(ItemsDictionary[item]));
            });

            try
            {
                var bodyImageModel = BodyImageService.GetBodyImage();
                var imageAsBytes = Convert.FromBase64String(bodyImageModel.ImageBase64String);
                BodyImage = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                BodyImageRegistedDateString = "登録日時 : " + bodyImageModel.RegistedDate;
            }
            catch (Exception)
            {
            }

            try
            {
                var model = BasicDataService.GetBasicData();
                Name = model.Name;
                Man = model.Sex;
                Age = model.Age;
                Height = model.Height;
                BodyWeight = model.BodyWeight;
                BodyFatPercentage = model.BodyFatPercentage;
                MaxBloodPressure = model.MaxBloodPressure;
                MinBloodPressure = model.MinBloodPressure;
                BasalMetabolism = model.BasalMetabolism;
            }
            catch (Exception)
            {
            }

            SetNewsSourceTask();
        }

        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

        public ICommand ItemTappedCommand { get; set; }
        public ICommand RegistBodyImageCommand { get; set; }
        public ICommand RegistBasicDataCommand { get; set; }
        public ICommand ViewBodyWeightChartCommand { get; set; }
        public ICommand ViewBodyFatPercentageChartCommand { get; set; }
        public ICommand ViewBodyImageListCommand { get; set; }

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

        // 名前
        public string Name { get; set; }

        public bool Man { get; }

        public bool Woman { get; }

        // 年齢
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

        // BMI
        public String Bmi
        {
            get
            {
                try
                {
                    var tmp = _bodyWeight / Math.Pow(_height / 100f, 2);
                    return CommonUtil.GetDecimalFormatString( double.IsNaN(tmp) ? 0 : tmp);
                }
                catch (Exception)
                {
                    return "0";
                }
            }
        }

        // 体脂肪率
        public float BodyFatPercentage { get; set; }

        // 上の血圧
        public int MaxBloodPressure { get; set; }

        // 下の血圧
        public int MinBloodPressure { get; set; }

        // 基礎代謝
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

        private async Task SetNewsSourceTask()
        {
            IsLoading = true;

            var service = NewsServiceFactory.CreateNewsService();

            ItemsDictionary = await service.GetNewsDictionary();

            ItemsDictionary.ForEach(data => Items.Add(data.Key));

            IsLoading = false;
        }

        private void RegistBodyImage()
        {
            ((App) Application.Current).ChangeScreen(new RegistBodyImageView());
        }

        private void RegistBasicData()
        {
            ((App) Application.Current).ChangeScreen(new InputBasicDataView());
        }

        private void ViewBodyWeightChart()
        {
            ((App) Application.Current).ChangeScreen(new DataChartView(BasicDataEnum.BodyWeight));
        }

        private void ViewBodyFatPercentageChart()
        {
            ((App) Application.Current).ChangeScreen(new DataChartView(BasicDataEnum.BodyFatPercentage));
        }

        private void ViewBodyImageList()
        {
            ((App) Application.Current).ChangeScreen(new BodyImageView());
        }
    }
}