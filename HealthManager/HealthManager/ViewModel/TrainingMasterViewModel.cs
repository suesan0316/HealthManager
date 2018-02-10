using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングマスター画面VMクラス
    /// </summary>
    public class TrainingMasterViewModel : INotifyPropertyChanged
    {

        private string _trainingName;

        private PartModel _part;

        private SubPartModel _subPart;

        private LoadModel _load;

        /// <summary>
        /// 読み込み中フラグ
        /// </summary>
        private bool _isLoading;

        public TrainingMasterViewModel()
        {
            CancleCommand = new Command(ViewModelCommonUtil.BackTrainingHome);
            SaveTrainingMasterCommand = new Command(async () => await SaveTrainingMaster());

            PartItemSrouce = PartService.GetPartDataList();
            SubPartService.GetSubPartDataList().ForEach(data => SubPartItemSrouce.Add(data));
            LoadService.GetLoadDataList().ForEach(data=>LoadItemSrouce.Add(data));
            Part = PartItemSrouce[0];
            SubPart = SubPartItemSrouce[0];
            Load = LoadItemSrouce[0];
        }

        /// <summary>
        /// 保存ボタンコマンド
        /// </summary>
        public ICommand SaveTrainingMasterCommand { get; }

        /// <summary>
        ///     キャンセルボタンコマンド
        /// </summary>
        public ICommand CancleCommand { get; }

        /// <summary>
        /// トレーニング名
        /// </summary>
        public string TrainingName
        {
            get => _trainingName;
            set
            {
                _trainingName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TrainingName)));
            }
        }

        /// <summary>
        /// 部位
        /// </summary>
        public PartModel Part
        {
            get => _part;
            set
            {
                _part = value;
                SubPartItemSrouce = new ObservableCollection<SubPartModel>();
                SubPartService.GetSubPartDataList(value.Id).ForEach(data => SubPartItemSrouce.Add(data));
                SubPart = SubPartItemSrouce[0];
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Part)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubPartItemSrouce)));
            }
        }

        /// <summary>
        /// サブ部位
        /// </summary>
        public SubPartModel SubPart
        {
            get => _subPart;
            set
            {
                _subPart = value ?? SubPartItemSrouce[0];
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubPart)));
            }
        }

        /// <summary>
        /// 負荷
        /// </summary>
        public LoadModel Load
        {
            get => _load;
            set
            {
                _load = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Load)));
            }
        }

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
        /// 無効フラグ
        /// </summary>
        public bool IsDisable => !_isLoading;

        /// <summary>
        ///     部位リストボックスアイテム
        /// </summary>
        public List<PartModel> PartItemSrouce { get; }

        /// <summary>
        ///     部位リストボックスアイテム
        /// </summary>
        public ObservableCollection<SubPartModel> SubPartItemSrouce { protected set; get; } =
            new ObservableCollection<SubPartModel>();

        /// <summary>
        ///     負荷リストボックスアイテム
        /// </summary>
        public ObservableCollection<LoadModel> LoadItemSrouce { protected set; get; } =
            new ObservableCollection<LoadModel>();

        /// <summary>
        ///     キャンセルボタンラベル
        /// </summary>
        public string CancelButtonLabel => LanguageUtils.Get(LanguageKeys.Cancel);

        /// <summary>
        /// 処理中ラベル
        /// </summary>
        public string LoadingLabel => LanguageUtils.Get(LanguageKeys.Loading);

        /// <summary>
        /// 保存ボタンラベル
        /// </summary>
        public string SaveButtonLabel => LanguageUtils.Get(LanguageKeys.Save);

        /// <summary>
        ///     トレーニング名ラベル
        /// </summary>
        public string TrainingNameLabel => LanguageUtils.Get(LanguageKeys.TrainingName);

        public string TrainingNamePlaceholderLabel => LanguageUtils.Get(LanguageKeys.InputTrainingName);

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
        private async Task SaveTrainingMaster()
        {
            try
            {
                IsLoading = true;
                if (BasicDataService.CheckExitTargetDayData(DateTime.Now))
                {
                    var result =
                        await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                        LanguageUtils.Get(LanguageKeys.TodayDataUpdateConfirm), LanguageUtils.Get(LanguageKeys.OK),
                            LanguageUtils.Get(LanguageKeys.Cancel));
                    if (result)
                    {
                        
                    }
                    else
                    {
                        IsLoading = false;
                        return;
                    }
                }
                else
                {
                    
                }

                IsLoading = false;
                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Complete),
                    LanguageUtils.Get(LanguageKeys.SaveComplete), LanguageUtils.Get(LanguageKeys.OK));
                ViewModelCommonUtil.BackHome();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}