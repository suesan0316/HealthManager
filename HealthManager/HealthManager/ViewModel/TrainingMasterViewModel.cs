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
using HealthManager.Common.Constant;
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

        /// <summary>
        /// トレーニング名
        /// </summary>
        private string _trainingName;

        public TrainingMasterViewModel()
        {
        }

        public TrainingMasterViewModel(StackLayout partStack, StackLayout loadStack)
        {
            PartStack = partStack;
            LoadStack = loadStack;

            CancleCommand = new Command(Cancel);
            SaveTrainingMasterCommand = new Command(async () => await SaveTrainingMaster());
            AddPartStackCommand = new Command(AddPartStack);
            DeletePartStackCommand = new Command(DeletePartStack);
            AddLoadStackCommand = new Command(AddLoadStack);
            DeleteLoadStackCommand = new Command(DeleteLoadStack);

            AddPartStack();
            AddLoadStack();
        }

        public TrainingMasterViewModel(StackLayout partStack, StackLayout loadStack, int id)
        {
            PartStack = partStack;
            LoadStack = loadStack;

            _targetTrainingMasterModel = TrainingMasterService.GetTrainingMasterData(id);
            CancleCommand = new Command(Cancel);
            SaveTrainingMasterCommand = new Command(async () => await SaveTrainingMaster());


            var partStructureList = JsonConvert.DeserializeObject<List<PartStructure>>(_targetTrainingMasterModel.Part);
            var loadStructure = JsonConvert.DeserializeObject<LoadStructure>(_targetTrainingMasterModel.Load);
            foreach (var data in partStructureList)
            {
                AddPartStack(data.Part.Id,data.SubPart.Id);
            }

            foreach (var data in loadStructure.LoadList)
            {
                AddLoadStack(data.Id);
            }

            TrainingName = _targetTrainingMasterModel.TrainingName;
        }

        /// <summary>
        ///     部位をスタックするレイアウト
        /// </summary>
        public StackLayout PartStack { get; set; }

        /// <summary>
        ///     負荷をスタックするレイアウト
        /// </summary>
        public StackLayout LoadStack { get; set; }

        /// <summary>
        ///     保存ボタンコマンド
        /// </summary>
        public ICommand SaveTrainingMasterCommand { get; }

        /// <summary>
        ///     部位を追加するボタンコマンド
        /// </summary>
        public ICommand AddPartStackCommand { get; }

        /// <summary>
        ///     部位を削除するボタンコマンド
        /// </summary>
        public ICommand DeletePartStackCommand { get; }

        /// <summary>
        ///     負荷を追加するボタンコマンド
        /// </summary>
        public ICommand AddLoadStackCommand { get; }

        /// <summary>
        ///     負荷を削除するボタンコマンド
        /// </summary>
        public ICommand DeleteLoadStackCommand { get; }

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

        /// <summary>
        ///     トレーニング名プレースホルダー
        /// </summary>
        public string TrainingNamePlaceholderLabel => LanguageUtils.Get(LanguageKeys.InputTrainingName);

        /// <summary>
        ///     部位追加ボタンラベル
        /// </summary>
        public string AddPartButtonLabel => LanguageUtils.Get(LanguageKeys.AddPart);

        /// <summary>
        ///     部位削除ボタンラベル
        /// </summary>
        public string DeletePartButtonLabel => LanguageUtils.Get(LanguageKeys.DeletePart);

        /// <summary>
        ///     部位追加ボタンラベル
        /// </summary>
        public string AddLoadButtonLabel => LanguageUtils.Get(LanguageKeys.AddLoad);

        /// <summary>
        ///     部位削除ボタンラベル
        /// </summary>
        public string DeleteLoadButtonLabel => LanguageUtils.Get(LanguageKeys.DeleteLoad);

        /// <summary>
        ///     トレーニングする部位のラベル
        /// </summary>
        public string TrainingPartLabel => LanguageUtils.Get(LanguageKeys.TrainingPart);

        /// <summary>
        ///     トレーニングの負荷のラベル
        /// </summary>
        public string TrainingLoadLabel => LanguageUtils.Get(LanguageKeys.TrainingLoad);

        /// <summary>
        /// エラーラベルをスタックするレイアウトのChildren
        /// </summary>
        public IList<Xamarin.Forms.View> ErrorStack { get; set; }

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

                if (!ValidationInputData(TrainingName))
                {
                    return;
                }

                IsLoading = true;

                var partStructureList = new List<PartStructure>();
                PartStack.Children.ForEach(child =>
                {
                    var stack = (StackLayout) child;
                    var partModel = ((PartModel)((Picker) stack.Children[0]).SelectedItem);
                    var subPartModel = ((SubPartModel)((Picker)stack.Children[1]).SelectedItem);

                    if (!partStructureList.Any(
                        data => data.Part.Id == partModel.Id && data.SubPart.Id == subPartModel.Id))
                    {
                        partStructureList.Add(new PartStructure
                        {
                            Part = partModel,
                            SubPart = subPartModel
                        });
                    }
                });

                var loadList = new List<LoadModel>();
                LoadStack.Children.ForEach(child =>
                {
                    var stack = (StackLayout) child;
                    var loadModel = ((LoadModel)((Picker)stack.Children[0]).SelectedItem);
                    if (loadList.All(data => data.Id != loadModel.Id))
                    {
                        loadList.Add(loadModel);
                    }
                });

                var loadStructure = new LoadStructure {LoadList = loadList};

                if (_targetTrainingMasterModel != null)
                    TrainingMasterService.UpdateTrainingMaster(
                        _targetTrainingMasterModel.Id,
                        TrainingName,
                        JsonConvert.SerializeObject(loadStructure),
                        JsonConvert.SerializeObject(partStructureList));
                else
                    TrainingMasterService.RegistTrainingMaster(
                        TrainingName,
                        JsonConvert.SerializeObject(loadStructure),
                        JsonConvert.SerializeObject(partStructureList));

                IsLoading = false;

                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Complete),
                    LanguageUtils.Get(LanguageKeys.SaveComplete), LanguageUtils.Get(LanguageKeys.OK));

                // ホーム画面をリロードする
                MessagingCenter.Send(this, ViewModelConst.MessagingHomeReload);
                ViewModelCommonUtil.TrainingBackPage();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public bool ValidationInputData(string trainingName)
        {
            ErrorStack.Clear();

            if (StringUtils.IsEmpty(trainingName))
            {
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.TrainingName, LanguageKeys.NotInputRequireData));
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
                Text = LanguageUtils.Get(errorType, LanguageUtils.Get(key)),
                TextColor = Color.Red
            };
        }

        private void Cancel()
        {
            // 遷移元画面をリロードする
            MessagingCenter.Send(this, ViewModelConst.MessagingHomeReload);
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }

        /// <summary>
        ///     部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreatePartPicker()
        {
            var pick = new Picker
            {
                ItemsSource = PartService.GetPartDataList(),
                ItemDisplayBinding = new Binding("PartName")
            };
            pick.SelectedIndex = 0;
            return pick;
        }

        /// <summary>
        ///     部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreatePartPicker(int id)
        {
            var pick = CreatePartPicker();
            pick.SelectedItem = ((List<PartModel>)pick.ItemsSource).First(data=>data.Id == id);
            return pick;
        }

        /// <summary>
        ///     サブ部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreateSubPartPicker()
        {
            var pick = new Picker
            {
                ItemsSource = SubPartService.GetSubPartDataList(),
                ItemDisplayBinding = new Binding("SubPartName")
            };
            pick.SelectedIndex = 0;
            return pick;
        }

        /// <summary>
        ///     サブ部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreateSubPartPicker(int id)
        {
            var pick = CreateSubPartPicker();
            pick.SelectedItem = ((List<SubPartModel>)pick.ItemsSource).First(data => data.Id == id);
            return pick;
        }

        /// <summary>
        ///     負荷ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreateLoadPicker()
        {
            var pick = new Picker
            {
                ItemsSource = LoadService.GetLoadDataList(),
                ItemDisplayBinding = new Binding("LoadName")
            };
            pick.SelectedIndex = 0;
            return pick;
        }

        /// <summary>
        ///     負荷ピッカー作成
        /// </summary>
        /// <returns></returns>
        private Picker CreateLoadPicker(int selectedIndex)
        {
            var pick = CreateLoadPicker();
            pick.SelectedItem = ((List<LoadModel>)pick.ItemsSource).First(data => data.Id == selectedIndex);
            return pick;
        }

        /// <summary>
        ///     部位のレイアウトを作成
        /// </summary>
        /// <returns></returns>
        private StackLayout CreatePartStackLayout()
        {
            var stack = new StackLayout();
            var partPicker = CreatePartPicker();
            var subPartPicker = CreateSubPartPicker();

            partPicker.SelectedIndexChanged += (sender, args) =>
            {
                var itemSource = new ObservableCollection<SubPartModel>();
                SubPartService.GetSubPartDataList(
                    ((PartModel) ((Picker) sender).SelectedItem).Id).ForEach(data => itemSource.Add(data));
                subPartPicker.ItemsSource = itemSource;
                subPartPicker.SelectedIndex = 0;
            };

            subPartPicker.ItemsSource = SubPartService.GetSubPartDataList(
                ((PartModel) partPicker.SelectedItem).Id);

            subPartPicker.SelectedIndex = 0;

            stack.Children.Add(partPicker);
            stack.Children.Add(subPartPicker);

            return stack;
        }

        /// <summary>
        ///     部位のレイアウトを作成
        /// </summary>
        /// <returns></returns>
        private StackLayout CreatePartStackLayout(int partId, int subPartId)
        {
            var stack = new StackLayout();
            var partPicker = CreatePartPicker(partId);
            var subPartPicker = CreateSubPartPicker(subPartId);

            partPicker.SelectedIndexChanged += (sender, args) =>
            {
                var itemSource = new ObservableCollection<SubPartModel>();
                SubPartService.GetSubPartDataList(
                    ((PartModel) ((Picker) sender).SelectedItem).Id).ForEach(data => itemSource.Add(data));
                subPartPicker.ItemsSource = itemSource;
                subPartPicker.SelectedIndex = 0;
            };

            stack.Children.Add(partPicker);
            stack.Children.Add(subPartPicker);
            return stack;
        }

        /// <summary>
        ///     負荷のレイアウトを作成
        /// </summary>
        /// <returns></returns>
        private StackLayout CreateLoadStackLayout()
        {
            var stack = new StackLayout();
            stack.Children.Add(CreateLoadPicker());
            return stack;
        }

        /// <summary>
        ///     負荷のレイアウトを作成
        /// </summary>
        /// <returns></returns>
        private StackLayout CreateLoadStackLayout(int loadId)
        {
            var stack = new StackLayout();
            stack.Children.Add(CreateLoadPicker(loadId));
            return stack;
        }

        /// <summary>
        ///     部位を追加するボタンアクション
        /// </summary>
        private void AddPartStack()
        {
            PartStack.Children.Add(CreatePartStackLayout());
        }

        /// <summary>
        ///     部位を追加するボタンアクション
        /// </summary>
        private void AddPartStack(int partId, int subPartId)
        {
            PartStack.Children.Add(CreatePartStackLayout(partId,subPartId));
        }

        /// <summary>
        ///     部位を削除するボタンアクション
        /// </summary>
        private void DeletePartStack()
        {
            if (PartStack.Children.Count != 1)
            {
                PartStack.Children.RemoveAt(PartStack.Children.Count - 1);
            }
        }

        /// <summary>
        ///     負荷を追加するボタンアクション
        /// </summary>
        private void AddLoadStack()
        {
            LoadStack.Children.Add(CreateLoadStackLayout());
        }

        /// <summary>
        ///     負荷を追加するボタンアクション
        /// </summary>
        private void AddLoadStack(int loadId)
        {
            LoadStack.Children.Add(CreateLoadStackLayout(loadId));
        }

        /// <summary>
        ///     負荷を削除するボタンアクション
        /// </summary>
        private void DeleteLoadStack()
        {
            if (LoadStack.Children.Count != 1)
            {
                LoadStack.Children.RemoveAt(LoadStack.Children.Count - 1);
            }
        }
    }
}