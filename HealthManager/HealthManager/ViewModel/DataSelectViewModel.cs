using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common.Enum;
using HealthManager.Common.Extention;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class DataSelectViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, BasicDataEnum> _dictionary = new Dictionary<string, BasicDataEnum>();

        private readonly List<BasicDataEnum> _showDataList = new List<BasicDataEnum>
        {
            BasicDataEnum.BasalMetabolism,
            BasicDataEnum.BodyWeight,
            BasicDataEnum.BodyFatPercentage,
            BasicDataEnum.MaxBloodPressure,
            BasicDataEnum.MinBloodPressure
        };

        public DataSelectViewModel()
        {
            BasicDataItemTappedCommand = new Command<string>(item =>
            {
                ((App) Application.Current).ChangeScreen(new DataChartView(_dictionary[item]));
            });
            foreach (var gender in Enum.GetValues(typeof(BasicDataEnum)))
                if (_showDataList.Contains((BasicDataEnum) gender))
                {
                    Items.Add(((BasicDataEnum) gender).DisplayString());
                    _dictionary.Add(((BasicDataEnum) gender).DisplayString(), (BasicDataEnum) gender);
                }
        }

        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

        public ICommand BasicDataItemTappedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}