using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HealthManager.Annotations;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class InputBasicDataViewModel : INotifyPropertyChanged
    {

        public InputBasicDataViewModel()
        {
            SaveBaisicDataCommand = new Command(async  ()=> await SaveBasicData());
        }

        // 名前
        private string _name;
        // 男性
        private bool _man = true;
        // 女性
        private bool _woman;
        // 年齢
        private int _age;
        // 身長
        private double _height;
        // 体重
        private double _bodyWeight;
        // 上の血圧
        private int _maxBloodPressure;
        // 下の血圧
        private int _minBloodPressure;

        private bool _isLoading = false;

        public Command SaveBaisicDataCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public bool Man
        {
            get { return _man; }
            set
            {
                _man = value;
                if (value)
                {
                    Woman = !value;
                    OnPropertyChanged(nameof(Man));
                    OnPropertyChanged(nameof(Woman));
                }
                else if(!value && !Woman)
                {
                    Woman = true;
                    OnPropertyChanged(nameof(Man));
                    OnPropertyChanged(nameof(Woman));
                }
            }
        }

        public bool Woman
        {
            get { return _woman; }
            set
            {
                _woman = value;
                if (value)
                {
                    Man = !value;
                    OnPropertyChanged(nameof(Woman));
                    OnPropertyChanged(nameof(Man));
                }
                else if(!value && !Man)
                {
                    Man = true;
                    OnPropertyChanged(nameof(Woman));
                    OnPropertyChanged(nameof(Man));
                }
            }
        }
        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value; 
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Bmi));
            }
        }

        public double BodyWeight
        {
            get { return _bodyWeight; }
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
                    var tmp = _bodyWeight / Math.Pow((_height / 100f), 2);
                    return double.IsNaN(tmp) ? 0 : tmp;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public int MaxBloodPressure
        {
            get { return _maxBloodPressure; }
            set { _maxBloodPressure = value; }
        }

        public int MinBloodPressure
        {
            get { return _minBloodPressure; }
            set { _minBloodPressure = value; }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(IsDisable));
            }
        }

        public bool IsDisable
        {
            get { return !_isLoading; }
        }

        async  Task SaveBasicData()
        {
            IsLoading = true;
            await Task.Delay(4000);
            IsLoading = false;
            await Application.Current.MainPage.DisplayAlert("完了", "保存が完了しました。","OK");
        }
    }
}
