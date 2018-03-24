using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.View;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     基本情報選択画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class DataSelectViewModel
    {
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Constractor

        public DataSelectViewModel()
        {
            InitCommands();
            foreach (var gender in Enum.GetValues(typeof(BasicDataEnum)))
                if (_showDataList.Contains((BasicDataEnum) gender))
                {
                    Items.Add(((BasicDataEnum) gender).DisplayString());
                    _dictionary.Add(((BasicDataEnum) gender).DisplayString(), (BasicDataEnum) gender);
                }
        }

        #endregion Constractor

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding Variables

        /// <summary>
        ///     基本情報リストアイテムソースリスト
        /// </summary>
        public ObservableCollection<string> Items { protected set; get; } = new ObservableCollection<string>();

        #endregion Binding Variables

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding DisplayLabels

        public string DisplayLabelReturn => LanguageUtils.Get(LanguageKeys.Return);

        #endregion Binding DisplayLabels

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Init Commands

        private void InitCommands()
        {
            CommandReturn =
                new Command(ViewModelCommonUtil.DataBackPage);
            CommandBasicDataItemTapped = new Command<string>(item =>
            {
                ViewModelConst.DataPageNavigation.PushAsync(new DataChartView(_dictionary[item]));
            });
        }

        #endregion Init Commands

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Class Variable
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Class Variable

        #endregion Class Variable

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Instance Private Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Instance Private Variables

        private readonly Dictionary<string, BasicDataEnum> _dictionary = new Dictionary<string, BasicDataEnum>();

        private readonly List<BasicDataEnum> _showDataList = new List<BasicDataEnum>
        {
            BasicDataEnum.BasalMetabolism,
            BasicDataEnum.BodyWeight,
            BasicDataEnum.BodyFatPercentage,
            BasicDataEnum.MaxBloodPressure,
            BasicDataEnum.MinBloodPressure
        };

        #endregion Instance Private Variables

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Binding Commands

        public ICommand CommandBasicDataItemTapped { get; set; }
        public ICommand CommandReturn { get; set; }

        #endregion Binding Commands

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Command Actions

        #endregion Command Actions

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region ViewModel Logic

        #endregion ViewModel Logic

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init MessageSubscribe
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Init MessageSubscribe

        //private void InitMessageSubscribe()
        //{

        //}

        #endregion Init MessageSubscribe

        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Default 
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/

        #region Default

        #endregion Default
    }
}