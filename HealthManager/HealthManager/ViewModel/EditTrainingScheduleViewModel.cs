using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
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
    ///     トレーニングスケジュール編集画面VM
    /// </summary>
    public class EditTrainingScheduleViewModel : INotifyPropertyChanged
    {

        private bool _isUpdate;

        public EditTrainingScheduleViewModel(int week, StackLayout trainingStack)
        {
            AddTrainingStackCommand = new Command(AddTrainingStack);
            DeleteTrainingStackCommand = new Command(DeleteTrainingStack);
            SaveTrainingScheduleCommand = new Command(SaveTrainingSchedule);
            CancleCommand = new Command(CancelAction);
            TrainingStack = trainingStack;
            WeekLabel = ((WeekEnum) week).ToString();

            var target = TrainingScheduleService.GetTrainingSchedule(week);
            if (target != null)
            {
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

            var trainingLoadStack = new StackLayout();

            var defaultLoad = JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem).Load);
            foreach (var load in defaultLoad.LoadList)
            {
                trainingLoadStack.Children.Add(new Label { Text = load.LoadName });
                trainingLoadStack.Children.Add(new Entry());
                trainingLoadStack.Children.Add(new Picker { ItemsSource = LoadUnitService.GetLoadUnitList(load.Id), ItemDisplayBinding = new Binding("UnitName"), SelectedIndex = 0 });
            }

            trainingPicker.SelectedIndexChanged += (sender, args) =>
            {
                trainingLoadStack.Children.Clear();
                var loadList = JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel) trainingPicker.SelectedItem).Load);
                foreach (var load in loadList.LoadList)
                {
                    trainingLoadStack.Children.Add(new Label { Text = load.LoadName });
                    trainingLoadStack.Children.Add(new Entry());
                    trainingLoadStack.Children.Add(new Picker { ItemsSource = LoadUnitService.GetLoadUnitList(load.Id), ItemDisplayBinding = new Binding("UnitName"), SelectedIndex = 0 });
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

            var trainingLoadStack = new StackLayout();

            var defaultLoad = JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem).Load);
            foreach (var load in defaultLoad.LoadList)
            {
                trainingLoadStack.Children.Add(new Label { Text = load.LoadName });
                trainingLoadStack.Children.Add(new Entry());
                trainingLoadStack.Children.Add(new Picker { ItemsSource = LoadUnitService.GetLoadUnitList(load.Id), ItemDisplayBinding = new Binding("UnitName"), SelectedIndex = 0 });
            }

            trainingPicker.SelectedIndexChanged += (sender, args) =>
            {
                trainingLoadStack.Children.Clear();
                var loadList = JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem).Load);
                foreach (var load in loadList.LoadList)
                {
                    trainingLoadStack.Children.Add(new Label { Text = load.LoadName });
                    trainingLoadStack.Children.Add(new Entry( ));
                    trainingLoadStack.Children.Add(new Picker {ItemsSource = LoadUnitService.GetLoadUnitList(load.Id),ItemDisplayBinding = new Binding("UnitName"),SelectedIndex = 0});
                }
            };

            mainStack.Children.Add(trainingLoadStack);
            TrainingStack.Children.Add(mainStack);
            trainingPicker.SelectedItem = ((List<TrainingMasterModel>)trainingPicker.ItemsSource).First(data => data.Id == training.TrainingId);
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

        public void SaveTrainingSchedule()
        {

        }
    }
}