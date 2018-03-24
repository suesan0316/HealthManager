using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.Model.Structure;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングマスター画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class TrainingMasterViewModel
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

        private readonly TrainingMasterModel _targetTrainingMasterModel;

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public TrainingMasterViewModel(StackLayout partStack, StackLayout loadStack)
        {
            PartStack = partStack;
            LoadStack = loadStack;
            InitCommands();
            CommandAddPartAction();
            CommandAddLoadAction();
        }

        public TrainingMasterViewModel(StackLayout partStack, StackLayout loadStack, int id)
        {
            PartStack = partStack;
            LoadStack = loadStack;

            _targetTrainingMasterModel = TrainingMasterService.GetTrainingMasterData(id);
           InitCommands();

            var partStructureList = JsonConvert.DeserializeObject<List<PartStructure>>(_targetTrainingMasterModel.Part);
            var loadStructure = JsonConvert.DeserializeObject<LoadStructure>(_targetTrainingMasterModel.Load);
            foreach (var data in partStructureList)
            {
                CommandAddPartAction(data.Part.Id, data.SubPart.Id);
            }

            foreach (var data in loadStructure.LoadList)
            {
                CommandAddLoadAction(data.Id);
            }

            TrainingName = _targetTrainingMasterModel.TrainingName;
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        /// <summary>
        ///     部位をスタックするレイアウト
        /// </summary>
        public StackLayout PartStack { get; set; }

        /// <summary>
        ///     負荷をスタックするレイアウト
        /// </summary>
        public StackLayout LoadStack { get; set; }

        /// <summary>
        ///     トレーニング名
        /// </summary>
        public string TrainingName { get; set; }

        /// <summary>
        ///     読み込みフラグ
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        ///     無効フラグ
        /// </summary>
        public bool IsDisable => !IsLoading;

        /// <summary>
        /// エラーラベルをスタックするレイアウトのChildren
        /// </summary>
        public IList<Xamarin.Forms.View> ErrorStack { get; set; }

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelAddLoad => LanguageUtils.Get(LanguageKeys.AddLoad);
        public string DisplayLabelAddPart => LanguageUtils.Get(LanguageKeys.AddPart);
        public string DisplayLabelCancel => LanguageUtils.Get(LanguageKeys.Cancel);
        public string DisplayLabelDeleteLoad => LanguageUtils.Get(LanguageKeys.DeleteLoad);
        public string DisplayLabelDeletePart => LanguageUtils.Get(LanguageKeys.DeletePart);
        public string DisplayLabelLoading => LanguageUtils.Get(LanguageKeys.Loading);
        public string DisplayLabelSave => LanguageUtils.Get(LanguageKeys.Save);
        public string DisplayLabelLoad => LanguageUtils.Get(LanguageKeys.TrainingLoad);
        public string DisplayLabelTrainingName => LanguageUtils.Get(LanguageKeys.TrainingName);
        public string DisplayLabelTrainingNamePlaceholder => LanguageUtils.Get(LanguageKeys.InputTrainingName);
        public string DisplayLabelPart => LanguageUtils.Get(LanguageKeys.TrainingPart);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        /// <summary>
        ///     保存ボタンコマンド
        /// </summary>
        public ICommand CommandSave { get; set; }

        /// <summary>
        ///     部位を追加するボタンコマンド
        /// </summary>
        public ICommand CommandAddPart { get; set; }

        /// <summary>
        ///     部位を削除するボタンコマンド
        /// </summary>
        public ICommand CommandDeletePart { get; set; }

        /// <summary>
        ///     負荷を追加するボタンコマンド
        /// </summary>
        public ICommand CommandAddLoad { get; set; }

        /// <summary>
        ///     負荷を削除するボタンコマンド
        /// </summary>
        public ICommand CommandDeleteLoad { get; set; }

        /// <summary>
        ///     キャンセルボタンコマンド
        /// </summary>
        public ICommand CommandCancel { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        /// <summary>
        ///     トレーニング保存アクション
        /// </summary>
        /// <returns></returns>
        private async Task CommandSaveAction()
        {
            try
            {

                if (!ValidationInputData(TrainingName))
                {
                    ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingSelfScroll);
                    return;
                }

                IsLoading = true;

                var partStructureList = new List<PartStructure>();
                PartStack.Children.ForEach(child =>
                {
                    var stack = (StackLayout)child;
                    var partModel = ((PartModel)((Picker)stack.Children[0]).SelectedItem);
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
                    var stack = (StackLayout)child;
                    var loadModel = ((LoadModel)((Picker)stack.Children[0]).SelectedItem);
                    if (loadList.All(data => data.Id != loadModel.Id))
                    {
                        loadList.Add(loadModel);
                    }
                });

                var loadStructure = new LoadStructure { LoadList = loadList };

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
                ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingPrevPageReload);
                ViewModelCommonUtil.TrainingBackPage();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        

        private void CommandCancelAction()
        {
            // 遷移元画面をリロードする
            ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingPrevPageReload);
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }


        /// <summary>
        ///     部位を追加するボタンアクション
        /// </summary>
        private void CommandAddPartAction()
        {
            PartStack.Children.Add(CreatePartStackLayout());
        }

        /// <summary>
        ///     部位を追加するボタンアクション
        /// </summary>
        private void CommandAddPartAction(int partId, int subPartId)
        {
            PartStack.Children.Add(CreatePartStackLayout(partId, subPartId));
        }

        /// <summary>
        ///     部位を削除するボタンアクション
        /// </summary>
        private void CommandDeletePartAction()
        {
            if (PartStack.Children.Count != 1)
            {
                PartStack.Children.RemoveAt(PartStack.Children.Count - 1);
            }
        }

        /// <summary>
        ///     負荷を追加するボタンアクション
        /// </summary>
        private void CommandAddLoadAction()
        {
            LoadStack.Children.Add(CreateLoadStackLayout());
        }

        /// <summary>
        ///     負荷を追加するボタンアクション
        /// </summary>
        private void CommandAddLoadAction(int loadId)
        {
            LoadStack.Children.Add(CreateLoadStackLayout(loadId));
        }

        /// <summary>
        ///     負荷を削除するボタンアクション
        /// </summary>
        private void CommandDeleteLoadAction()
        {
            if (LoadStack.Children.Count != 1)
            {
                LoadStack.Children.RemoveAt(LoadStack.Children.Count - 1);
            }
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
            CommandCancel = new Command(CommandCancelAction);
            CommandSave = new Command(async () => await CommandSaveAction());
            CommandAddPart = new Command(CommandAddPartAction);
            CommandDeletePart = new Command(CommandDeletePartAction);
            CommandAddLoad = new Command(CommandAddLoadAction);
            CommandDeleteLoad = new Command(CommandDeleteLoadAction);
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

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

        /// <summary>
        ///     部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private static Picker CreatePartPicker()
        {
            var pick = new Picker
            {
                ItemsSource = PartService.GetPartDataList(),
                ItemDisplayBinding = new Binding("PartName"),
                SelectedIndex = 0
            };
            return pick;
        }

        /// <summary>
        ///     部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private static Picker CreatePartPicker(int id)
        {
            var pick = CreatePartPicker();
            pick.SelectedItem = ((List<PartModel>)pick.ItemsSource).First(data => data.Id == id);
            return pick;
        }

        /// <summary>
        ///     サブ部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private static Picker CreateSubPartPicker()
        {
            var pick = new Picker
            {
                ItemsSource = SubPartService.GetSubPartDataList(),
                ItemDisplayBinding = new Binding("SubPartName"),
                SelectedIndex = 0
            };
            return pick;
        }

        /// <summary>
        ///     サブ部位ピッカー作成
        /// </summary>
        /// <returns></returns>
        private static Picker CreateSubPartPicker(int id)
        {
            var pick = CreateSubPartPicker();
            pick.SelectedItem = ((List<SubPartModel>)pick.ItemsSource).First(data => data.Id == id);
            return pick;
        }

        /// <summary>
        ///     負荷ピッカー作成
        /// </summary>
        /// <returns></returns>
        private static Picker CreateLoadPicker()
        {
            var pick = new Picker
            {
                ItemsSource = LoadService.GetLoadDataList(),
                ItemDisplayBinding = new Binding("LoadName"),
                SelectedIndex = 0
            };
            return pick;
        }

        /// <summary>
        ///     負荷ピッカー作成
        /// </summary>
        /// <returns></returns>
        private static Picker CreateLoadPicker(int selectedIndex)
        {
            var pick = CreateLoadPicker();
            pick.SelectedItem = ((List<LoadModel>)pick.ItemsSource).First(data => data.Id == selectedIndex);
            return pick;
        }

        /// <summary>
        ///     部位のレイアウトを作成
        /// </summary>
        /// <returns></returns>
        private static StackLayout CreatePartStackLayout()
        {
            var stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
            var partPicker = CreatePartPicker();
            var subPartPicker = CreateSubPartPicker();

            // TODO 一旦これでしのぐ
            partPicker.WidthRequest = 145;
            subPartPicker.WidthRequest = 145;

            partPicker.SelectedIndexChanged += (sender, args) =>
            {
                var itemSource = new ObservableCollection<SubPartModel>();
                SubPartService.GetSubPartDataList(
                    ((PartModel)((Picker)sender).SelectedItem).Id).ForEach(data => itemSource.Add(data));
                subPartPicker.ItemsSource = itemSource;
                subPartPicker.SelectedIndex = 0;
            };

            subPartPicker.ItemsSource = SubPartService.GetSubPartDataList(
                ((PartModel)partPicker.SelectedItem).Id);

            subPartPicker.SelectedIndex = 0;

            stack.Children.Add(partPicker);
            stack.Children.Add(subPartPicker);

            return stack;
        }

        /// <summary>
        ///     部位のレイアウトを作成
        /// </summary>
        /// <returns></returns>
        private static StackLayout CreatePartStackLayout(int partId, int subPartId)
        {
            var stack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
            var partPicker = CreatePartPicker(partId);
            var subPartPicker = CreateSubPartPicker(subPartId);

            partPicker.WidthRequest = 145;
            subPartPicker.WidthRequest = 145;

            partPicker.SelectedIndexChanged += (sender, args) =>
            {
                var itemSource = new ObservableCollection<SubPartModel>();
                SubPartService.GetSubPartDataList(
                    ((PartModel)((Picker)sender).SelectedItem).Id).ForEach(data => itemSource.Add(data));
                subPartPicker.ItemsSource = itemSource;
                subPartPicker.SelectedIndex = 0;
            };

            subPartPicker.ItemsSource = SubPartService.GetSubPartDataList(
                ((PartModel)partPicker.SelectedItem).Id);

            partPicker.SelectedItem = ((List<PartModel>)partPicker.ItemsSource).First(data => data.Id == partId);
            subPartPicker.SelectedItem = ((List<SubPartModel>)subPartPicker.ItemsSource).First(data => data.Id == subPartId);

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

        public bool ValidationInputData(string trainingName)
        {
            ErrorStack.Clear();

            if (StringUtils.IsEmpty(trainingName))
            {
                ErrorStack.Add(CreateErrorLabel(LanguageKeys.TrainingName, LanguageKeys.NotInputRequireData));
            }
            return ErrorStack.Count == 0;
        }

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