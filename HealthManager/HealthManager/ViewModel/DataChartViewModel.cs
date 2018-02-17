using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Properties;
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

        /// <summary>
        /// データチャートエントリーリスト
        /// </summary>
        private readonly List<Entry> _entries = new List<Entry>();

        /// <summary>
        /// 対象の基本情報列挙
        /// </summary>
        private readonly BasicDataEnum _targetBasicDataEnum;

        /// <summary>
        /// デフォルトのコンストラクタは使用しない想定
        /// </summary>
        public DataChartViewModel() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetBasicDataEnum"></param>
        public DataChartViewModel(BasicDataEnum targetBasicDataEnum)
        {
            BackHomeCommand = new Command(ViewModelCommonUtil.DataBackPage);

            _targetBasicDataEnum = targetBasicDataEnum;

            // 一度全データを取得
            var list = BasicDataService.GetBasicDataList();

            // 指定された列挙値によって取得する値を決定する
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
            DataList = _entries.Select(data => data.Label + StringConst.Blank + data.Value + _targetBasicDataEnum.DisplayUnit());
        }

        /// <summary>
        /// 戻るボタンコマンド
        /// </summary>
        public ICommand BackHomeCommand { get; set; }

        /// <summary>
        /// 画面に表示されるチャート
        /// </summary>
        public Chart Chart { get; set; }

        /// <summary>
        /// 期間ラベル
        /// </summary>
        public string TermText
        {
            get {
				return LanguageUtils.Get(LanguageKeys.Terms)
					+ _entries.Min(data => data.Label) 
					+ StringConst.WavyLine 
					+ _entries.Max(data => data.Label); 
            }
        }

        /// <summary>
        /// 期間最小ラベル
        /// </summary>
        public string TermMinText
        {
            get { return LanguageUtils.Get(LanguageKeys.TermsMin) + _entries.Min(data => data.Value) + _targetBasicDataEnum.DisplayUnit(); }
        }

        /// <summary>
        /// 期間最大ラベル
        /// </summary>
        public string TermMaxText
        {
            get { return LanguageUtils.Get(LanguageKeys.TermsMax) +　 _entries.Max(data => data.Value) + _targetBasicDataEnum.DisplayUnit(); }
        }

        /// <summary>
        /// 期間平均ラベル
        /// </summary>
        public string TermAverageText
        {
            get { return LanguageUtils.Get(LanguageKeys.TermsAverage) + _entries.Average(data => data.Value) + _targetBasicDataEnum.DisplayUnit(); }
        }

        /// <summary>
        /// チャートしたに表示される一覧のデータ
        /// </summary>
        public IEnumerable<string> DataList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 戻るボタンラベル
        /// </summary>
        public string BackHomeLabel => LanguageUtils.Get(LanguageKeys.Return);

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