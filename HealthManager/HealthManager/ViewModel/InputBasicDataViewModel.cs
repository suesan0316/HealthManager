using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HealthManager.Annotations;
using HealthManager.Model.Service;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class InputBasicDataViewModel : INotifyPropertyChanged
    {
        // 体重
        private float _bodyWeight;

        // 身長
        private float _height;

        // 読み込み中フラグ
        private bool _isLoading;

        // 男性
        private bool _man = true;

        // 女性
        private bool _woman;

        private bool _cancelButtonIsVisible;

        public InputBasicDataViewModel()
        {
            SaveBaisicDataCommand = new Command(async () => await SaveBasicData());
            CancleCommand = new Command(Cancel);

            CancelButtonIsVisible = true;

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
            catch (Exception e)
            {
                CancelButtonIsVisible = false;
            }
        }

        public Command SaveBaisicDataCommand { get; }
        public Command CancleCommand { get; }

        // 名前
        public string Name { get; set; }

        public bool Man
        {
            get => _man;
            set
            {
                _man = value;
                if (value)
                {
                    Woman = false;
                    OnPropertyChanged(nameof(Man));
                    OnPropertyChanged(nameof(Woman));
                }
                else if (!Woman)
                {
                    Woman = true;
                    OnPropertyChanged(nameof(Man));
                    OnPropertyChanged(nameof(Woman));
                }
            }
        }

        public bool Woman
        {
            get => _woman;
            set
            {
                _woman = value;
                if (value)
                {
                    Man = false;
                    OnPropertyChanged(nameof(Woman));
                    OnPropertyChanged(nameof(Man));
                }
                else if (!Man)
                {
                    Man = true;
                    OnPropertyChanged(nameof(Woman));
                    OnPropertyChanged(nameof(Man));
                }
            }
        }

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
        public double Bmi
        {
            get
            {
                try
                {
                    var tmp = _bodyWeight / Math.Pow(_height / 100f, 2);
                    return double.IsNaN(tmp) ? 0 : tmp;
                }
                catch (Exception)
                {
                    return 0;
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
                OnPropertyChanged(nameof(IsDisable));
            }
        }

        public bool CancelButtonIsVisible
        {
            get => _cancelButtonIsVisible;
            set
            {
                _cancelButtonIsVisible = value;
                OnPropertyChanged(nameof(CancelButtonIsVisible));
            }
        }

        public bool IsDisable => !_isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task SaveBasicData()
        {
            IsLoading = true;
            await Task.Delay(1000);
            BasicDataService.RegistBasicData(Name, Man, height: Height, age: Age,
                bodyWeight: BodyWeight, bodyFatPercentage: BodyFatPercentage, maxBloodPressure: MaxBloodPressure,
                minBloodPressure: MinBloodPressure, basalMetabolism: BasalMetabolism);
            IsLoading = false;
            await Application.Current.MainPage.DisplayAlert("完了", "保存が完了しました。", "OK");
            ((App) Application.Current).ChangeScreen(new HomeView());
        }

        private void Cancel()
        {
            ((App)Application.Current).ChangeScreen(new HomeView());
        }

    }
}