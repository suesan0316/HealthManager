using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.View;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     基本情報選択画面VMクラス
    /// </summary>
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
            BackPageCommand =
                new Command(ViewModelCommonUtil.DataBackPage);
            BasicDataItemTappedCommand = new Command<string>(item =>
            {
                ViewModelConst.DataPageNavigation.PushAsync(new DataChartView(_dictionary[item]));
            });
            foreach (var gender in Enum.GetValues(typeof(BasicDataEnum)))
                if (_showDataList.Contains((BasicDataEnum) gender))
                {
                    Items.Add(((BasicDataEnum) gender).DisplayString());
                    _dictionary.Add(((BasicDataEnum) gender).DisplayString(), (BasicDataEnum) gender);
                }
        }

        /// <summary>
        ///     基本情報リストアイテムソースリスト
        /// </summary>
        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

        /// <summary>
        ///     リストアイテムタップコマンド
        /// </summary>
        public ICommand BasicDataItemTappedCommand { get; set; }

        /// <summary>
        ///     戻るボタンコマンド
        /// </summary>
        public ICommand BackPageCommand { get; set; }

        /// <summary>
        ///     戻るボタンラベル
        /// </summary>
        public string BackPageLabel => LanguageUtils.Get(LanguageKeys.Return);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}