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

        public InputBasicDataViewModel()
        {
            SaveBaisicDataCommand = new Command(async  ()=> await SaveBasicData());
        }

        public Command SaveBaisicDataCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 名前
        public string Name { get; set; }


        // 男性
        private bool _man = true;
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
                else if(!Woman)
                {
                    Woman = true;
                    OnPropertyChanged(nameof(Man));
                    OnPropertyChanged(nameof(Woman));
                }
            }
        }

        // 女性
        private bool _woman;
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
                else if(!Man)
                {
                    Man = true;
                    OnPropertyChanged(nameof(Woman));
                    OnPropertyChanged(nameof(Man));
                }
            }
        }

        // 年齢
        public int Age { get; set; }

        // 身長
        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                _height = value; 
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Bmi));
            }
        }

        // 体重
        private double _bodyWeight;
        public double BodyWeight
        {
            get => _bodyWeight;
            set
            {
                _bodyWeight = value; 
                OnPropertyChanged(nameof(BodyWeight));
                OnPropertyChanged(nameof(Bmi));
            }
        }

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

        // 上の血圧
        public int MaxBloodPressure { get; set; }

        // 下の血圧
        public int MinBloodPressure { get; set; }


        // 読み込み中フラグ
        private bool _isLoading;
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

        public bool IsDisable => !_isLoading;

        private async  Task SaveBasicData()
        {
            IsLoading = true;
            await Task.Delay(4000);
             //InputBasicDataService.RegistBasicData(name: Name, sex: Man, height: Height, age: Age,
                //bodyWeight:BodyWeight, maxBloodPressure: MaxBloodPressure, minBloodPressure: MinBloodPressure);
            IsLoading = false;
            await Application.Current.MainPage.DisplayAlert("完了", "保存が完了しました。","OK");
            ((App)Application.Current).ChangeScreen(new HomeView());
        }
    }
}
