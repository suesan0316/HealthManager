using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Extention;
using HealthManager.Model.Service;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// データチャート画面のVMクラス
    /// </summary>
    internal class DataChartViewModel : INotifyPropertyChanged
    {
        private readonly List<Entry> _entries = new List<Entry>();

        private readonly BasicDataEnum _targetBasicDataEnum;

        public DataChartViewModel(BasicDataEnum targetBasicDataEnum)
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.BackHome);

            _targetBasicDataEnum = targetBasicDataEnum;

            var list = BasicDataService.GetBasicDataList();

            switch (_targetBasicDataEnum)
            {
                case BasicDataEnum.BodyWeight:
                    _entries = list.Select(value => CreateNewEntry(value.BodyWeight, value.RegistedDate))
                        .ToList();
                    break;

                case BasicDataEnum.BodyFatPercentage:
                    _entries = list.Select(value => CreateNewEntry(value.BodyFatPercentage, value.RegistedDate))
                        .ToList();
                    break;
                case BasicDataEnum.Name:
                case BasicDataEnum.Sex:
                case BasicDataEnum.Age:
                    break;
                case BasicDataEnum.Height:
                    _entries = list.Select(value => CreateNewEntry(value.Height, value.RegistedDate))
                        .ToList();
                    break;
                case BasicDataEnum.MaxBloodPressure:
                    _entries = list.Select(value => CreateNewEntry(value.MaxBloodPressure, value.RegistedDate))
                        .ToList();
                    break;
                case BasicDataEnum.MinBloodPressure:
                    _entries = list.Select(value => CreateNewEntry(value.MinBloodPressure, value.RegistedDate))
                        .ToList();
                    break;
                case BasicDataEnum.BasalMetabolism:
                    _entries = list.Select(value => CreateNewEntry(value.BasalMetabolism, value.RegistedDate)).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Chart = new LineChart {Entries = _entries};
            DataList = _entries.Select(data => data.Label + "  " + data.Value + _targetBasicDataEnum.DisplayUnit());
        }

        public ICommand BackHomeCommand { get; set; }

        public Chart Chart { get; set; }

        public string TermText
        {
            get {
				return "期間 : " 
					+ _entries.Min(data => data.Label) 
					+ "~" 
					+ _entries.Max(data => data.Label); 
            }
        }

        public string TermMinText
        {
            get { return "期間中最小 : " + _entries.Min(data => data.Value) + _targetBasicDataEnum.DisplayUnit(); }
        }

        public string TermMaxText
        {
            get { return "期間中最大 : " +　 _entries.Max(data => data.Value) + _targetBasicDataEnum.DisplayUnit(); }
        }

        public string TermAverageText
        {
            get { return "期間中平均 : " + _entries.Average(data => data.Value) + _targetBasicDataEnum.DisplayUnit(); }
        }

        public IEnumerable<string> DataList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// データチャートに使用するエントリーを生成
        /// </summary>
        /// <param name="value"></param>
        /// <param name="registedDateTime"></param>
        /// <returns></returns>
        protected virtual Entry CreateNewEntry(float value, DateTime registedDateTime)
        {
            return new Entry(value)
            {
                // TODO 色を決定
                Color = SKColor.Parse("#00CED1"),
                Label = ViewModelCommonUtil.FormatDateString(registedDateTime),
                ValueLabel = value.ToString()
            };
        }
    }
}