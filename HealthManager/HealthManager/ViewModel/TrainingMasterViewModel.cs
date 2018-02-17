using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.Model.Structure;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングマスター画面VMクラス
    /// </summary>
    public class TrainingMasterViewModel : INotifyPropertyChanged
    {
        private readonly TrainingMasterModel _targetTrainingMasterModel;

        /// <summary>
        ///     読み込み中フラグ
        /// </summary>
        private bool _isLoading;

        private LoadModel _load;

        private PartModel _part;

        private SubPartModel _subPart;

        private string _trainingName;

        private IList<Picker> _partList;

        private IList<Picker> _loadList;

        public TrainingMasterViewModel()
        {
            CancleCommand = new Command(ViewModelCommonUtil.BackTrainingHome);
            SaveTrainingMasterCommand = new Command(async () => await SaveTrainingMaster());

            PartItemSrouce = PartService.GetPartDataList();
            SubPartService.GetSubPartDataList().ForEach(data => SubPartItemSrouce.Add(data));
            LoadService.GetLoadDataList().ForEach(data => LoadItemSrouce.Add(data));
            Part = PartItemSrouce[0];
            SubPart = SubPartItemSrouce[0];
            Load = LoadItemSrouce[0];
        }

        public TrainingMasterViewModel(int id)
        {
            _targetTrainingMasterModel = TrainingMasterService.GetTrainingMasterData(id);
            CancleCommand = new Command(ViewModelCommonUtil.BackTrainingHome);
            SaveTrainingMasterCommand = new Command(async () => await SaveTrainingMaster());

            PartItemSrouce = PartService.GetPartDataList();
            SubPartService.GetSubPartDataList().ForEach(data => SubPartItemSrouce.Add(data));
            LoadService.GetLoadDataList().ForEach(data => LoadItemSrouce.Add(data));

            var partStructure = JsonConvert.DeserializeObject<PartStructure>(_targetTrainingMasterModel.Part);
            var loadStructure = JsonConvert.DeserializeObject<LoadStructure>(_targetTrainingMasterModel.Load);

            TrainingName = _targetTrainingMasterModel.TrainingName;
            Part = PartItemSrouce.First(data => data.Id == partStructure.Part.Id);
            SubPart = SubPartItemSrouce.First(data => data.Id == partStructure.SubPart.Id);
            Load = LoadItemSrouce.First(data => data.Id == loadStructure.LoadList[0].Id);

        }

        /// <summary>
        ///     保存ボタンコマンド
        /// </summary>
        public ICommand SaveTrainingMasterCommand { get; }

        /// <summary>
        ///     キャンセルボタンコマンド
        /// </summary>
        public ICommand CancleCommand { get; }

        /// <summary>
        ///     トレーニング名
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
        ///     部位
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
        ///     サブ部位
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
        ///     負荷
        /// </summary>
        public LoadModel Load
        {
            get => _load;
            set
            {
                _load = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Load)));
            }
        }

        /// <summary>
        ///     読み込みフラグ
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
        ///     無効フラグ
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
        ///     処理中ラベル
        /// </summary>
        public string LoadingLabel => LanguageUtils.Get(LanguageKeys.Loading);

        /// <summary>
        ///     保存ボタンラベル
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
        ///     トレーニング保存アクション
        /// </summary>
        /// <returns></returns>
        private async Task SaveTrainingMaster()
        {
            try
            {
                IsLoading = true;
                var partStructure = new PartStructure {Part = Part, SubPart = SubPart};
                var loadStructure = new LoadStructure {LoadList = new List<LoadModel> {Load}};


                if (_targetTrainingMasterModel != null)
                    TrainingMasterService.UpdateTrainingMaster(
                        _targetTrainingMasterModel.Id,
                        TrainingName,
                        JsonConvert.SerializeObject(loadStructure),
                        JsonConvert.SerializeObject(partStructure));
                else
                    TrainingMasterService.RegistTrainingMaster(
                        TrainingName,
                        JsonConvert.SerializeObject(loadStructure),
                        JsonConvert.SerializeObject(partStructure));

                IsLoading = false;

                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Complete),
                    LanguageUtils.Get(LanguageKeys.SaveComplete), LanguageUtils.Get(LanguageKeys.OK));
                ViewModelCommonUtil.BackHome();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// 部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreatePartPicker()
        {
            var pick = new Picker
            {
                ItemsSource = PartItemSrouce
            };

            return pick;

        }

        /// <summary>
        /// サブ部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreateSubPartPicker()
        {
            var pick = new Picker
            {
                ItemsSource = SubPartItemSrouce
            };

            return pick;

        }

        /// <summary>
        /// 負荷ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreateLoadPicker()
        {
            var pick = new Picker
            {
                ItemsSource = SubPartItemSrouce
            };

            return pick;

        }

    }
}