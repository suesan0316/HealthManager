﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// コンストラクタ
        /// 基本データが存在しない場合はキャンセルボタンを非表示
        /// </summary>
        public InputBasicDataViewModel()
        {
            SaveBaisicDataCommand = new Command(async () => await SaveBasicData());
            CancleCommand = new Command(ViewModelCommonUtil.BackHome);

            GenderItemSrouce = new List<GenderEnum>();
            foreach (var gender in Enum.GetValues(typeof(GenderEnum)))
            {
                GenderItemSrouce.Add((GenderEnum)gender);
            }

            Gender = (int)GenderEnum.未選択;

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
            }
            else
            {
                CancelButtonIsVisible = false;
            }
        }

        public ICommand SaveBaisicDataCommand { get; }
        public ICommand CancleCommand { get; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年齢
        /// </summary>
        public int Age { get; set; }

        public List<GenderEnum> GenderItemSrouce { get; }

        /// <summary>
        /// 性別
        /// </summary>
        public int Gender { get; set; }

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
        /// 体脂肪率
        /// </summary>
        public float BodyFatPercentage { get; set; }

        /// <summary>
        /// 上の血圧
        /// </summary>
        public int MaxBloodPressure { get; set; }

        /// <summary>
        /// 下の血圧
        /// </summary>
        public int MinBloodPressure { get; set; }

        /// <summary>
        /// 下の血圧
        /// </summary>
        public int BasalMetabolism { get; set; }

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
                IsLoading = true;
                if (BasicDataService.CheckExitTargetDayData(DateTime.Now))
                {
                    var result =
                        await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm), LanguageUtils.Get(LanguageKeys.TodayDataUpdateConfirm), LanguageUtils.Get(LanguageKeys.OK),
                            LanguageUtils.Get(LanguageKeys.Cancel));
                    if (result)
                        BasicDataService.UpdateBasicData(_id, Name, gender:Gender, height: Height, age: Age,
                            bodyWeight: BodyWeight, bodyFatPercentage: BodyFatPercentage,
                            maxBloodPressure: MaxBloodPressure,
                            minBloodPressure: MinBloodPressure, basalMetabolism: BasalMetabolism);
                }
                else
                {
                    BasicDataService.RegistBasicData(Name, gender:Gender, height: Height, age: Age,
                        bodyWeight: BodyWeight, bodyFatPercentage: BodyFatPercentage,
                        maxBloodPressure: MaxBloodPressure,
                        minBloodPressure: MinBloodPressure, basalMetabolism: BasalMetabolism);
                }

                IsLoading = false;
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Complete), LanguageUtils.Get(LanguageKeys.SaveComplete),LanguageUtils.Get(LanguageKeys.OK));
                ViewModelCommonUtil.BackHome();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}