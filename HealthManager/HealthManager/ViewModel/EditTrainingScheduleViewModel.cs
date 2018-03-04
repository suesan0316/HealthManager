using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using HealthManager.Model;
using HealthManager.Model.Service;
using HealthManager.Model.Structure;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングスケジュール編集画面VM
    /// </summary>
    public class EditTrainingScheduleViewModel : INotifyPropertyChanged
    {

        private bool _isUpdate;

        private readonly int _week;

        private readonly int _id;

        public EditTrainingScheduleViewModel(int week, StackLayout trainingStack)
        {
            AddTrainingStackCommand = new Command(AddTrainingStack);
            DeleteTrainingStackCommand = new Command(DeleteTrainingStack);
            SaveTrainingScheduleCommand = new Command(async() => await  SaveTrainingSchedule());
            CancleCommand = new Command(CancelAction);
            TrainingStack = trainingStack;
            WeekLabel = ((WeekEnum) week).ToString();
            _week = week;

            var target = TrainingScheduleService.GetTrainingSchedule(week);
            if (target != null)
            {
                _isUpdate = true;
                _id = target.Id;
                var trainingList = JsonConvert.DeserializeObject<TrainingScheduleStructure>(target.TrainingMenu).TrainingContentList;

                foreach (var training in trainingList)
                {
                    AddTrainingStack(training);
                }
            }
        }

        public string WeekLabel { get; set; }

        /// <summary>
        ///     エラーラベルをスタックするレイアウトのChildren
        /// </summary>
        public IList<Xamarin.Forms.View> ErrorStack { get; set; }

        public string OffSwitchLabel => LanguageUtils.Get(LanguageKeys.Rest);

        public bool Off { get; set; }

        public StackLayout TrainingStack { get; set; }

        public string AddTrainingButtonLabel => LanguageUtils.Get(LanguageKeys.AddTraining);

        public string DeleteTrainingButtonLabel => LanguageUtils.Get(LanguageKeys.DeleteTraining);

        public ICommand AddTrainingStackCommand { get; set; }

        public ICommand DeleteTrainingStackCommand { get; set; }

        public string SaveButtonLabel => LanguageUtils.Get(LanguageKeys.Save);

        public string CancelButtonLabel => LanguageUtils.Get(LanguageKeys.Cancel);

        public ICommand SaveTrainingScheduleCommand { get; set; }

        public ICommand CancleCommand { get; set; }

        public bool isLoading { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddTrainingStack()
        {
            var mainStack = new StackLayout();
            var trainingLabel = new Label {Text = LanguageUtils.Get(LanguageKeys.TrainingName)};
            mainStack.Children.Add(trainingLabel);
            var trainingPicker = new Picker
            {
                ItemsSource = TrainingMasterService.GetTrainingMasterDataList(),
                ItemDisplayBinding = new Binding("TrainingName")
            };
            trainingPicker.SelectedIndex = 0;
            mainStack.Children.Add(trainingPicker);

            var trainingSetCountLabel = new Label { Text = LanguageUtils.Get(LanguageKeys.SetCount) };
            mainStack.Children.Add(trainingSetCountLabel);

            var trainingSetCountEntry = new Entry();
            mainStack.Children.Add(trainingSetCountEntry);

            var trainingLoadStack = new StackLayout();

            var defaultLoad = JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem).Load);
            foreach (var load in defaultLoad.LoadList)
            {
                var loadStack = new StackLayout();
                loadStack.Children.Add(new Label { Text = load.LoadName });
                loadStack.Children.Add(new Entry());
                loadStack.Children.Add(new Picker { ItemsSource = LoadUnitService.GetLoadUnitList(load.Id), ItemDisplayBinding = new Binding("UnitName"), SelectedIndex = 0 });
                trainingLoadStack.Children.Add(loadStack);
            }

            trainingPicker.SelectedIndexChanged += (sender, args) =>
            {
                trainingLoadStack.Children.Clear();
                var loadList = JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel) trainingPicker.SelectedItem).Load);
                foreach (var load in loadList.LoadList)
                {
                    var loadStack = new StackLayout();
                    loadStack.Children.Add(new Label { Text = load.LoadName });
                    loadStack.Children.Add(new Entry());
                    loadStack.Children.Add(new Picker { ItemsSource = LoadUnitService.GetLoadUnitList(load.Id), ItemDisplayBinding = new Binding("UnitName"), SelectedIndex = 0 });
                    trainingLoadStack.Children.Add(loadStack);
                }
            };

            mainStack.Children.Add(trainingLoadStack);
            TrainingStack.Children.Add(mainStack);
            trainingPicker.SelectedIndex = 0;
        }

        public void AddTrainingStack(TrainingListStructure training)
        {
            var mainStack = new StackLayout();
            var trainingLabel = new Label { Text = LanguageUtils.Get(LanguageKeys.TrainingName) };
            mainStack.Children.Add(trainingLabel);
            var trainingPicker = new Picker
            {
                ItemsSource = TrainingMasterService.GetTrainingMasterDataList(),
                ItemDisplayBinding = new Binding("TrainingName")
            };
            trainingPicker.SelectedIndex = 0;
            mainStack.Children.Add(trainingPicker);

            var trainingSetCountLabel = new Label {Text = LanguageUtils.Get(LanguageKeys.SetCount)};
            mainStack.Children.Add(trainingSetCountLabel);

            var trainingSetCountEntry = new Entry{Text = training.TrainingSetCount.ToString()};
            mainStack.Children.Add(trainingSetCountEntry);

            var trainingLoadStack = new StackLayout();


            trainingPicker.SelectedItem = ((List<TrainingMasterModel>)trainingPicker.ItemsSource).First(data => data.Id == training.TrainingId);

            foreach (var load in training.LoadContentList)
            {
                var loadModdel = LoadService.GetLoad(load.LoadId);
                var loadUnitModel = LoadUnitService.GetLoadUnit(load.LoadUnitId);

                var loadStack = new StackLayout();
                loadStack.Children.Add(new Label { Text = loadModdel.LoadName });
                loadStack.Children.Add(new Entry {Text = load.Nums.ToString()});
                var loadUnitPick = new Picker
                {
                    ItemsSource = LoadUnitService.GetLoadUnitList(load.LoadId),
                    ItemDisplayBinding = new Binding("UnitName"),
                    SelectedIndex = 0
                };
                loadUnitPick.SelectedItem =
                    ((List<LoadUnitModel>) loadUnitPick.ItemsSource).First(data => data.Id == load.LoadUnitId);
                loadStack.Children.Add(loadUnitPick);
                trainingLoadStack.Children.Add(loadStack);
            }

            trainingPicker.SelectedIndexChanged += (sender, args) =>
            {
                trainingLoadStack.Children.Clear();
                var loadList = JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem).Load);
                foreach (var load in loadList.LoadList)
                {
                    var loadStack = new StackLayout();
                    loadStack.Children.Add(new Label { Text = load.LoadName });
                    loadStack.Children.Add(new Entry( ));
                    loadStack.Children.Add(new Picker {ItemsSource = LoadUnitService.GetLoadUnitList(load.Id),ItemDisplayBinding = new Binding("UnitName"),SelectedIndex = 0});
                    trainingLoadStack.Children.Add(loadStack);
                }
            };

            mainStack.Children.Add(trainingLoadStack);
            TrainingStack.Children.Add(mainStack);
        }

        public void DeleteTrainingStack()
        {
            if (TrainingStack.Children.Count != 1)
            {
                TrainingStack.Children.RemoveAt(TrainingStack.Children.Count - 1);
            }
        }

        public void CancelAction()
        {
            // 遷移元画面をリロードする
            MessagingCenter.Send(this, ViewModelConst.MessagingHomeReload);
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }

        private async Task SaveTrainingSchedule()
        {
            try
            {
                //TODO バリデータを作成
                if (1==2)
                {
                    return;
                }

                isLoading = true;

                var trainingContentList = new List<TrainingListStructure>();

                //トレーニング一覧
                var trainingStack = TrainingStack.Children;
                foreach (var training in trainingStack)
                {
                    var insert = new TrainingListStructure();
                    var trainingId = ((TrainingMasterModel)((Picker)((StackLayout) training).Children[1]).SelectedItem).Id;
                    var trainingseCount = ((Entry) ((StackLayout) training).Children[3]).Text;

                    var loadContentList = new List<LoadContentStructure>();
                    var loadStack = ((StackLayout)((StackLayout)training).Children[4]).Children;
                    foreach (var load in loadStack)
                    {
                        var insertload = new LoadContentStructure();
                        var loadId = ((LoadUnitModel)((Picker)((StackLayout)load).Children[2]).SelectedItem).LoadId;
                        var nums = ((Entry)((StackLayout)load).Children[1]).Text;
                        var loadUnitId = ((LoadUnitModel)((Picker)((StackLayout)load).Children[2]).SelectedItem).Id;
                        insertload.LoadId = loadId;
                        insertload.LoadUnitId = loadUnitId;
                        insertload.Nums = float.Parse(nums);
                        loadContentList.Add(insertload);
                    }

                    insert.LoadContentList = loadContentList;
                    insert.TrainingId = trainingId;
                    insert.TrainingSetCount = int.Parse(trainingseCount);
                    trainingContentList.Add(insert);
                }

                var trainingScheduleStructure = new TrainingScheduleStructure();
                trainingScheduleStructure.TrainingContentList = trainingContentList;
                trainingScheduleStructure.Off = Off;
                trainingScheduleStructure.Week = _week;

                var trainingScheduleStructureJson = JsonConvert.SerializeObject(trainingScheduleStructure);

                if (_isUpdate)
                {
                    TrainingScheduleService.UpdateTrainingSchedule(id: _id, trainingMenu: trainingScheduleStructureJson,
                        week: _week);
                }
                else
                {
                    TrainingScheduleService.RegistTrainingSchedule(trainingMenu: trainingScheduleStructureJson,
                        week: _week);
                }

                isLoading = false;

                await Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Complete),
                    LanguageUtils.Get(LanguageKeys.SaveComplete), LanguageUtils.Get(LanguageKeys.OK));

                // ホーム画面をリロードする
                MessagingCenter.Send(this, ViewModelConst.MessagingHomeReload);
                ViewModelCommonUtil.TrainingBackPage();

            }
            catch (Exception e)
            {

            }
        }
    }
}