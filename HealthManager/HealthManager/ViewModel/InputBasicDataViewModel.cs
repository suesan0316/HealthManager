using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Model.Service;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    public class InputBasicDataViewModel : INotifyPropertyChanged
    {
        private readonly int _id;

        // 体重
        private float _bodyWeight;

        // キャンセルボタン表示フラグ
        private bool _cancelButtonIsVisible;

        // 身長
        private float _height;

        // 読み込み中フラグ
        private bool _isLoading;

        /// <summary>
        ///     コンストラクタ
        ///     基本データが存在しない場合はキャンセルボタンを非表示
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

        // 名前
        public string Name { get; set; }

        // 年齢
        public int Age { get; set; }

        public List<GenderEnum> GenderItemSrouce { get; }

        public int Gender { get; set; }

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

        // BMI
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
                    return "0";
                }
            }
        }

        // 体脂肪率
        public float BodyFatPercentage { get; set; }

        // 上の血圧
        public int MaxBloodPressure { get; set; }

        // 下の血圧
        public int MinBloodPressure { get; set; }

        // 基礎代謝
        public int BasalMetabolism { get; set; }

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
        ///     基本データ保存アクション
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
                        await Application.Current.MainPage.DisplayAlert("確認", "本日は既にデータを登録しています。更新しますか？", "OK",
                            "キャンセル");
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
                await Application.Current.MainPage.DisplayAlert("完了", "保存が完了しました。", "OK");
                ViewModelCommonUtil.BackHome();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}