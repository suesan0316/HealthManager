using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Properties;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    /// 基本情報登録画面VMクラス
    /// </summary>
    public class InputBasicDataViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 基本情報ID
        /// </summary>
        private readonly int _id;

        /// <summary>
        /// 体重
        /// </summary>
        private float _bodyWeight;

        /// <summary>
        /// キャンセルボタン表示フラグ
        /// </summary>
        private bool _cancelButtonIsVisible;

        /// <summary>
        /// 身長
        /// </summary>
        private float _height;

        /// <summary>
        /// 読み込み中フラグ
        /// </summary>
        private bool _isLoading;

        /// <summary>
        /// 更新フラグ
        /// </summary>
        private readonly bool _isUpdate;        

        /// <summary>
        /// コンストラクタ
        /// 基本データが存在しない場合はキャンセルボタンを非表示
        /// </summary>
        public InputBasicDataViewModel()
        {
            try
            {
                SaveBaisicDataCommand = new Command(async () => await SaveBasicData());
                CancleCommand = new Command(ViewModelCommonUtil.BackHome);
                GenderItemSrouce = new List<GenderEnum>();
                foreach (var gender in Enum.GetValues(typeof(GenderEnum)))
                {
                    GenderItemSrouce.Add((GenderEnum) gender);
                }

                Gender = (int) GenderEnum.未選択;

                CancelButtonIsVisible = true;

                var model = BasicDataService.GetBasicData();
                if (model != null)
                {
                    _id = model.Id;
                    Name = model.Name;
                    Gender = model.Gender;
                    Age = model.Age;
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
                    CancelButtonIsVisible = false;
                }

                ErrorStack.Clear();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// 保存ボタンコマンド
        /// </summary>
        public ICommand SaveBaisicDataCommand { get; }

        /// <summary>
        /// キャンセルボタンコマンド
        /// </summary>
        public ICommand CancleCommand { get; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名前ラベル
        /// </summary>
        public string NameLabel => LanguageUtils.Get(LanguageKeys.Name);

        /// <summary>
        /// 名前プレースホルダーラベル
        /// </summary>
        public string NamePlaceholderLabel => LanguageUtils.Get(LanguageKeys.NamePlaceholder);

        /// <summary>
        /// 年齢
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 年齢ラベル
        /// </summary>
        public string AgeLabel => LanguageUtils.Get(LanguageKeys.Age);

        /// <summary>
        /// 年齢プレースホルダーラベル
        /// </summary>
        public string AgePlaceholderLabel => LanguageUtils.Get(LanguageKeys.AgePlaceholder);

        /// <summary>
        /// 性別リストボックスアイテム
        /// </summary>
        public List<GenderEnum> GenderItemSrouce { get; }

        /// <summary>
        /// 性別
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 性別ラベル
        /// </summary>
        public string GenderLabel => LanguageUtils.Get(LanguageKeys.Gender);

        /// <summary>
        /// エラーラベルをスタックするレイアウトのChildren
        /// </summary>
        public IList<Xamarin.Forms.View> ErrorStack { get; set; } 

        /// <summary>
        /// 身長
        /// </summary>
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

        /// <summary>
        /// 身長ラベル
        /// </summary>
        public string HeightLabel => LanguageUtils.Get(LanguageKeys.Height);

        /// <summary>
        /// 身長プレースホルダーラベル
        /// </summary>
        public string HeightPlaceholderLabel => LanguageUtils.Get(LanguageKeys.HeightPlaceholder);

        /// <summary>
        /// 体重
        /// </summary>
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

        /// <summary>
        /// 体重ラベル
        /// </summary>
        public string BodyWeightLabel => LanguageUtils.Get(LanguageKeys.BodyWeight);

        /// <summary>
        /// 体重プレースホルダーラベル
        /// </summary>
        public string BodyWeightPlaceholderLabel => LanguageUtils.Get(LanguageKeys.BodyWeightPlaceHolder);

        /// <summary>
        /// BMI(自動計算項目)
        /// </summary>
        public string Bmi
        {
            get
            {
                try
                {
                    var tmp = _bodyWeight / Math.Pow(_height / 100f, 2);
                    return CommonUtil.GetDecimalFormatString(double.IsNaN(tmp) ? 0 : tmp);
                }
                catch (Exception)
                {
                    return StringConst.Zero;
                }
            }
        }

        /// <summary>
        /// BMIラベル
        /// </summary>
        public string BmiLabel => LanguageUtils.Get(LanguageKeys.BMI);

        /// <summary>
        /// 体脂肪率
        /// </summary>
        public float BodyFatPercentage { get; set; }

        /// <summary>
        /// 体脂肪率ラベル
        /// </summary>
        public string BodyFatPercentageLabel => LanguageUtils.Get(LanguageKeys.BodyFatPercentage);

        /// <summary>
        /// 体脂肪率プレースホルダーラベル
        /// </summary>
        public string BodyFatPercentagePlaceholderLabel => LanguageUtils.Get(LanguageKeys.BodyFatPercentagePlaceholder);

        /// <summary>
        /// 血圧ラベル
        /// </summary>
        public string BloodPressureLabel => LanguageUtils.Get(LanguageKeys.BloodPressure);
        
        /// <summary>
        /// 上の血圧
        /// </summary>
        public int MaxBloodPressure { get; set; }

        /// <summary>
        /// 上の血圧プレースホルダーラベル
        /// </summary>
        public string MaxBloodPressurePlaceholderLabel => LanguageUtils.Get(LanguageKeys.MaxBloodPressure);

        /// <summary>
        /// 下の血圧
        /// </summary>
        public int MinBloodPressure { get; set; }

        /// <summary>
        /// 下の血圧プレースホルダーラベル
        /// </summary>
        public string MinBloodPressurePlaceholderLabel => LanguageUtils.Get(LanguageKeys.MinBloodPressure);

        /// <summary>
        /// 基礎代謝
        /// </summary>
        public int BasalMetabolism { get; set; }

        /// <summary>
        /// 基礎代謝ラベル
        /// </summary>
        public string BasalMetabolismLabel => LanguageUtils.Get(LanguageKeys.BasicMetabolism);

        /// <summary>
        /// 基礎代謝プレースホルダーラベル
        /// </summary>
        public string BasalMetabolismPlaceholderLabel => LanguageUtils.Get(LanguageKeys.BasalMetabolismPlaceholder);

        /// <summary>
        /// 読み込みフラグ
        /// </summary>
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

        /// <summary>
        /// 処理中ラベル
        /// </summary>
        public string LoadingLabel => LanguageUtils.Get(LanguageKeys.Loading);

        /// <summary>
        /// 保存ボタンラベル
        /// </summary>
        public string SaveButtonLabel => LanguageUtils.Get(LanguageKeys.Save);

        /// <summary>
        /// キャンセルボタンラベル
        /// </summary>
        public string CancelButtonLabel => LanguageUtils.Get(LanguageKeys.Cancel);

        /// <summary>
        /// キャンセルボタン非表示・表示
        /// </summary>
        public bool CancelButtonIsVisible
        {
            get => _cancelButtonIsVisible;
            set
            {
                _cancelButtonIsVisible = value;
                OnPropertyChanged(nameof(CancelButtonIsVisible));
            }
        }

        /// <summary>
        /// 無効フラグ
        /// </summary>
        public bool IsDisable => !_isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///  基本データ保存アクション
        /// </summary>
        /// <returns></returns>
        private async Task SaveBasicData()
        {
            try
            {

                if (!ValidationInputData(Name, gender: Gender, height: Height, age: Age,
                    bodyWeight: BodyWeight, bodyFatPercentage: BodyFatPercentage,
                    maxBloodPressure: MaxBloodPressure,
                    minBloodPressure: MinBloodPressure, basalMetabolism: BasalMetabolism))
                {
                    return;
                }

                IsLoading = true;
                if (BasicDataService.CheckExitTargetDayData(DateTime.Now))
                {
                    var result =
                        await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                        LanguageUtils.Get(LanguageKeys.TodayDataUpdateConfirm), LanguageUtils.Get(LanguageKeys.OK),
                            LanguageUtils.Get(LanguageKeys.Cancel));
                    if (result)
                    {
                        BasicDataService.UpdateBasicData(_id, Name, gender: Gender, height: Height, age: Age,
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
                    BasicDataService.RegistBasicData(Name, gender:Gender, height: Height, age: Age,
                        bodyWeight: BodyWeight, bodyFatPercentage: BodyFatPercentage,
                        maxBloodPressure: MaxBloodPressure,
                        minBloodPressure: MinBloodPressure, basalMetabolism: BasalMetabolism);
                }

                IsLoading = false;
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Complete),
                    LanguageUtils.Get(LanguageKeys.SaveComplete),LanguageUtils.Get(LanguageKeys.OK));
                if (_isUpdate)
                {
                    // ホーム画面をリロードする
                    MessagingCenter.Send(this, ViewModelConst.MessagingHomeReload);
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
        /// 入力項目のバリデーションチェックを実施します
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="age"></param>
        /// <param name="height"></param>
        /// <param name="bodyWeight"></param>
        /// <param name="bodyFatPercentage"></param>
        /// <param name="maxBloodPressure"></param>
        /// <param name="minBloodPressure"></param>
        /// <param name="basalMetabolism"></param>
        /// <returns></returns>
        public bool ValidationInputData(string name, int gender, int age, float height, float bodyWeight,
            float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure, int basalMetabolism)
        {

            ErrorStack.Clear();

            if (StringUtils.IsEmpty(name))
            {
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.Name,LanguageKeys.NotInputRequireData));
            }

            if (age <= 0)
            {
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.Age, LanguageKeys.NotAvailableDataInput));
            }

            if (height <= 0)
            {
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.Height, LanguageKeys.NotAvailableDataInput));
            }

            if (bodyWeight <= 0)
            {
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.BodyWeight, LanguageKeys.NotAvailableDataInput));
            }

            return ErrorStack.Count == 0;
        }

        /// <summary>
        /// エラーラベルを生成します
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public Label CreateErrorLabel(string key, string errorType)
        {
            return new Label
            {
                Text = LanguageUtils.Get(errorType,LanguageUtils.Get(key)),
                TextColor = Color.Red
            };
        }
    }
}