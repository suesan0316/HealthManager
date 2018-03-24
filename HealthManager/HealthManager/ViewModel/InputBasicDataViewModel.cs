using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model.Service.Interface;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///  基本情報登録画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class InputBasicDataViewModel
    {
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
            
        private readonly bool _isUpdate;
        private readonly int _id;

        private readonly IBasicDataService _basicDataService;

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public InputBasicDataViewModel(IBasicDataService basicDataService)
        {
            try
            {
                _basicDataService = basicDataService;

                InitCommands();
                 GenderItemSrouce = new List<GenderEnum>();
                foreach (var gender in Enum.GetValues(typeof(GenderEnum))) GenderItemSrouce.Add((GenderEnum)gender);

                Gender = (int)GenderEnum.未選択;

                CancelButtonIsVisible = true;

                var model = _basicDataService.GetBasicData();
                if (model != null)
                {
                    _id = model.Id;
                    Name = model.Name;
                    Gender = model.Gender;
                    Birthday = model.BirthDay;
                    Height = model.Height;
                    BodyWeight = model.BodyWeight;
                    BodyFatPercentage = model.BodyFatPercentage;
                    MaxBloodPressure = model.MaxBloodPressure;
                    MinBloodPressure = model.MinBloodPressure;
                    BasalMetabolism = model.BasalMetabolism;

                    _isUpdate = true;
                }
                else
                {
                    Birthday = DateTime.Parse(ViewModelConst.DefaultTimePick);
                    CancelButtonIsVisible = false;
                }             

                ErrorStack = new ObservableCollection<Xamarin.Forms.View>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        public ScrollRequest ScrollRequest { get; set; }

        /// <summary>
        ///  名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  性別リストボックスアイテム
        /// </summary>
        public List<GenderEnum> GenderItemSrouce { get; }

        /// <summary>
        ///  性別
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        ///  エラーラベルをスタックするレイアウトのChildren
        /// </summary>
        public ObservableCollection<Xamarin.Forms.View> ErrorStack { get; set; }

        /// <summary>
        ///  身長
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        ///  体重
        /// </summary>
        public float BodyWeight { get; set; }

        /// <summary>
        ///  BMI(自動計算項目)
        /// </summary>
        public string Bmi
        {
            get
            {
                try
                {
                    var tmp = BodyWeight / Math.Pow(Height / 100f, 2);
                    return CommonUtil.GetDecimalFormatString(double.IsNaN(tmp) ? 0 : tmp);
                }
                catch (Exception)
                {
                    return StringConst.Zero;
                }
            }
        }

        /// <summary>
        ///  体脂肪率
        /// </summary>
        public float BodyFatPercentage { get; set; }

        /// <summary>
        ///  上の血圧
        /// </summary>
        public int MaxBloodPressure { get; set; }

        /// <summary>
        ///  下の血圧
        /// </summary>
        public int MinBloodPressure { get; set; }

        /// <summary>
        ///  基礎代謝
        /// </summary>
        public int BasalMetabolism { get; set; }

        /// <summary>
        /// 生年月日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        ///  読み込みフラグ
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        ///  キャンセルボタン非表示・表示
        /// </summary>
        public bool CancelButtonIsVisible { get; set; }

        /// <summary>
        ///  無効フラグ
        /// </summary>
        public bool IsDisable => !IsLoading;

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelBasalMetabolism => LanguageUtils.Get(LanguageKeys.BasicMetabolism);
        public string DisplayLabelBasalMetabolismPlaceholder => LanguageUtils.Get(LanguageKeys.BasalMetabolismPlaceholder);
        public string DisplayLabelBirthday => LanguageUtils.Get(LanguageKeys.Birthday);
        public string DisplayLabelBloodPressure => LanguageUtils.Get(LanguageKeys.BloodPressure);
        public string DisplayLabelBmi => LanguageUtils.Get(LanguageKeys.BMI);
        public string DisplayLabelBodyFatPercentage => LanguageUtils.Get(LanguageKeys.BodyFatPercentage);
        public string DisplayLabelBodyFatPercentagePlaceholder => LanguageUtils.Get(LanguageKeys.BodyFatPercentagePlaceholder);
        public string DisplayLabelBodyWeight => LanguageUtils.Get(LanguageKeys.BodyWeight);
        public string DisplayLabelBodyWeightPlaceholder => LanguageUtils.Get(LanguageKeys.BodyWeightPlaceHolder);
        public string DisplayLabelCancel => LanguageUtils.Get(LanguageKeys.Cancel);
        public string DisplayLabelGender => LanguageUtils.Get(LanguageKeys.Gender);
        public string DisplayLabelHeight => LanguageUtils.Get(LanguageKeys.Height);
        public string DisplayLabelHeightPlaceholder => LanguageUtils.Get(LanguageKeys.HeightPlaceholder);
        public string DisplayLabelLoading => LanguageUtils.Get(LanguageKeys.Loading);
        public string DisplayLabelMaxBloodPressurePlaceholder => LanguageUtils.Get(LanguageKeys.MaxBloodPressure);
        public string DisplayLabelMinBloodPressurePlaceholder => LanguageUtils.Get(LanguageKeys.MinBloodPressure);
        public string DisplayLabelName => LanguageUtils.Get(LanguageKeys.Name);
        public string DisplayLabelNamePlaceholder => LanguageUtils.Get(LanguageKeys.NamePlaceholder);
        public string DisplayLabelSave => LanguageUtils.Get(LanguageKeys.Save);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        public ICommand CommandCancel { get; set; }
        public ICommand CommandSave { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        /// <summary>
        ///  基本データ保存アクション
        /// </summary>
        /// <returns></returns>
        private async Task CommandSaveAction()
        {
            try
            {
                if (!ValidationInputData(Name, Gender, Height,
                    BodyWeight, BodyFatPercentage,
                    MaxBloodPressure,
                    MinBloodPressure, BasalMetabolism))
                {
                    ScrollRequest = ScrollRequest.SendScrollRequest(ScrollRequestType.RequestTypeToTop);
                    return;
                }

                IsLoading = true;
                if (_basicDataService.CheckExitTargetDayData(DateTime.Now))
                {
                    var result =
                        await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                            LanguageUtils.Get(LanguageKeys.TodayDataUpdateConfirm), LanguageUtils.Get(LanguageKeys.OK),
                            LanguageUtils.Get(LanguageKeys.Cancel));
                    if (result)
                    {
                        _basicDataService.UpdateBasicData(_id, Name, 1, Gender, height: Height, birthday: Birthday,
                            bodyWeight: BodyWeight, bodyFatPercentage: BodyFatPercentage,
                            maxBloodPressure: MaxBloodPressure,
                            minBloodPressure: MinBloodPressure, basalMetabolism: BasalMetabolism);
                    }
                    else
                    {
                        IsLoading = false;
                        return;
                    }
                }
                else
                {
                    _basicDataService.RegistBasicData(Name, 1, Gender, height: Height, birthday: Birthday,
                        bodyWeight: BodyWeight, bodyFatPercentage: BodyFatPercentage,
                        maxBloodPressure: MaxBloodPressure,
                        minBloodPressure: MinBloodPressure, basalMetabolism: BasalMetabolism);
                }

                IsLoading = false;
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Complete),
                    LanguageUtils.Get(LanguageKeys.SaveComplete), LanguageUtils.Get(LanguageKeys.OK));
                if (_isUpdate)
                {
                    // ホーム画面をリロードする
                    ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingHomeReload);
                    ViewModelCommonUtil.DataBackPage();
                }
                else
                {
                    ViewModelCommonUtil.BackHome();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        ///  キャンセルアクション
        /// </summary>
        public void CommandCancelAction()
        {
            // ホーム画面をリロードする
            ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingHomeReload);
            ViewModelCommonUtil.DataBackPage();
        }

        #endregion Command Actions
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init Commands

        private void InitCommands()
        {
            CommandSave = new Command(async () => await CommandSaveAction());
            CommandCancel = new Command(CommandCancelAction);
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

        /// <summary>
        ///  入力項目のバリデーションチェックを実施します
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="height"></param>
        /// <param name="bodyWeight"></param>
        /// <param name="bodyFatPercentage"></param>
        /// <param name="maxBloodPressure"></param>
        /// <param name="minBloodPressure"></param>
        /// <param name="basalMetabolism"></param>
        /// <returns></returns>
        public bool ValidationInputData(string name, int gender, float height, float bodyWeight,
            float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure, int basalMetabolism)
        {
            ErrorStack.Clear();

            if (StringUtils.IsEmpty(name))
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.Name, LanguageKeys.NotInputRequireData));

            if (height <= 0) ErrorStack.Add(CreateErrorLabel(LanguageKeys.Height, LanguageKeys.NotAvailableDataInput));

            if (bodyWeight <= 0)
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.BodyWeight, LanguageKeys.NotAvailableDataInput));

            return ErrorStack.Count == 0;
        }

        /// <summary>
        ///  エラーラベルを生成します
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public Label CreateErrorLabel(string key, string errorType)
        {
            return new Label
            {
                Text = LanguageUtils.Get(errorType, LanguageUtils.Get(key)),
                TextColor = Color.Red
            };
        }

        #endregion ViewModel Logic
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init MessageSubscribe
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init MessageSubscribe

/*
        private void InitMessageSubscribe()
        {

        }
*/

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