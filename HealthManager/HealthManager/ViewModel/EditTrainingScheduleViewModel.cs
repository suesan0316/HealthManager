using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
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
    ///     トレーニングスケジュール編集画面VM
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class EditTrainingScheduleViewModel
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

        private readonly int _id;
        private readonly bool _isUpdate;
        private readonly int _week;

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public EditTrainingScheduleViewModel(int week, StackLayout trainingStack)
        {
            InitCommands();
            TrainingStack = trainingStack;
            WeekLabel = ((WeekEnum)week).ToString();
            _week = week;

            var target = TrainingScheduleService.GetTrainingSchedule(week);
            if (target != null)
            {
                _isUpdate = true;
                _id = target.Id;
                var trainingScheduleStructure =
                    JsonConvert.DeserializeObject<TrainingScheduleStructure>(target.TrainingMenu);

                Off = trainingScheduleStructure.Off;

                if (!Off)
                    foreach (var training in trainingScheduleStructure.TrainingContentList)
                        CommandAddTrainingAction(training);
                else
                    CommandAddTrainingAction();
            }
            else
            {
                CommandAddTrainingAction();
            }
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables
       
        /// <summary>
        ///     エラーラベルをスタックするレイアウトのChildren
        /// </summary>
        public IList<Xamarin.Forms.View> ErrorStack { get; set; }
        public bool IsLoading { get; set; }
        public bool Off { get; set; }
        public StackLayout TrainingStack { get; set; }
        public string WeekLabel { get; set; }

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelOff => LanguageUtils.Get(LanguageKeys.Rest);
        public string DisplayLabelAddTraining => LanguageUtils.Get(LanguageKeys.AddTraining);
        public string DisplayLabelDeleteTraining => LanguageUtils.Get(LanguageKeys.DeleteTraining);
        public string DisplayLabelSave => LanguageUtils.Get(LanguageKeys.Save);
        public string DisplayLabelCancel => LanguageUtils.Get(LanguageKeys.Cancel);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        public ICommand CommandAddTraining { get; set; }
        public ICommand CommandDeleteTraining { get; set; }
        public ICommand CommandSave { get; set; }
        public ICommand CommandCancel { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        public void CommandAddTrainingAction()
        {
            var mainStack = new StackLayout();
            var trainingLabel = new Label
            {
                Text = LanguageUtils.Get(LanguageKeys.TrainingName),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 20, 0, 0)
            };
            mainStack.Children.Add(trainingLabel);
            var trainingPicker = new Picker
            {
                ItemsSource = TrainingMasterService.GetTrainingMasterDataList(),
                ItemDisplayBinding = new Binding("TrainingName"),
                SelectedIndex = 0
            };
            mainStack.Children.Add(trainingPicker);

            var trainingSetCountLabel = new Label
            {
                Text = LanguageUtils.Get(LanguageKeys.SetCount),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 0)
            };
            mainStack.Children.Add(trainingSetCountLabel);

            var trainingSetCountEntry = new Entry { Keyboard = Keyboard.Numeric };
            mainStack.Children.Add(trainingSetCountEntry);

            var trainingLoadStack = new StackLayout();

            var defaultLoad =
                JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem).Load);
            foreach (var load in defaultLoad.LoadList)
            {
                var loadStack = new StackLayout();
                loadStack.Children.Add(new Label
                {
                    Text = load.LoadName,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 0, 0, 0)
                });

                var subLoadStack = new StackLayout { Orientation = StackOrientation.Horizontal };
                subLoadStack.Children.Add(new Entry { Keyboard = Keyboard.Numeric, WidthRequest = 145 });
                subLoadStack.Children.Add(new Picker
                {
                    ItemsSource = LoadUnitService.GetLoadUnitList(load.Id),
                    ItemDisplayBinding = new Binding("UnitName"),
                    SelectedIndex = 0,
                    WidthRequest = 145
                });
                loadStack.Children.Add(subLoadStack);

                trainingLoadStack.Children.Add(loadStack);
            }

            trainingPicker.SelectedIndexChanged += (sender, args) =>
            {
                trainingLoadStack.Children.Clear();
                var loadList =
                    JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem)
                        .Load);
                foreach (var load in loadList.LoadList)
                {
                    var loadStack = new StackLayout();
                    loadStack.Children.Add(new Label
                    {
                        Text = load.LoadName,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(0, 0, 0, 0)
                    });

                    var subLoadStack = new StackLayout { Orientation = StackOrientation.Horizontal };
                    subLoadStack.Children.Add(new Entry { Keyboard = Keyboard.Numeric, WidthRequest = 145 });
                    subLoadStack.Children.Add(new Picker
                    {
                        ItemsSource = LoadUnitService.GetLoadUnitList(load.Id),
                        ItemDisplayBinding = new Binding("UnitName"),
                        SelectedIndex = 0,
                        WidthRequest = 145
                    });
                    loadStack.Children.Add(subLoadStack);
                    trainingLoadStack.Children.Add(loadStack);
                }
            };

            mainStack.Children.Add(trainingLoadStack);
            TrainingStack.Children.Add(mainStack);
            trainingPicker.SelectedIndex = 0;
        }

        public void CommandAddTrainingAction(TrainingListStructure training)
        {
            var mainStack = new StackLayout();
            var trainingLabel = new Label
            {
                Text = LanguageUtils.Get(LanguageKeys.TrainingName),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 20, 0, 0)
            };
            mainStack.Children.Add(trainingLabel);
            var trainingPicker = new Picker
            {
                ItemsSource = TrainingMasterService.GetTrainingMasterDataList(),
                ItemDisplayBinding = new Binding("TrainingName"),
                SelectedIndex = 0
            };
            mainStack.Children.Add(trainingPicker);

            var trainingSetCountLabel = new Label
            {
                Text = LanguageUtils.Get(LanguageKeys.SetCount),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 0, 0)
            };
            mainStack.Children.Add(trainingSetCountLabel);

            var trainingSetCountEntry =
                new Entry { Text = training.TrainingSetCount.ToString(), Keyboard = Keyboard.Numeric };
            mainStack.Children.Add(trainingSetCountEntry);

            var trainingLoadStack = new StackLayout();


            trainingPicker.SelectedItem =
                ((List<TrainingMasterModel>)trainingPicker.ItemsSource).First(data => data.Id == training.TrainingId);

            foreach (var load in training.LoadContentList)
            {
                var loadModdel = LoadService.GetLoad(load.LoadId);

                var loadStack = new StackLayout();
                loadStack.Children.Add(new Label
                {
                    Text = loadModdel.LoadName,
                    FontAttributes = FontAttributes.Bold,
                    Margin = new Thickness(0, 0, 0, 0)
                });

                var subLoadStack = new StackLayout { Orientation = StackOrientation.Horizontal };
                subLoadStack.Children.Add(new Entry
                {
                    Text = load.Nums.ToString(),
                    Keyboard = Keyboard.Numeric,
                    WidthRequest = 145
                });
                var loadUnitPick = new Picker
                {
                    ItemsSource = LoadUnitService.GetLoadUnitList(load.LoadId),
                    ItemDisplayBinding = new Binding("UnitName"),
                    SelectedIndex = 0,
                    WidthRequest = 145
                };
                loadUnitPick.SelectedItem =
                    ((List<LoadUnitModel>)loadUnitPick.ItemsSource).First(data => data.Id == load.LoadUnitId);

                subLoadStack.Children.Add(loadUnitPick);

                loadStack.Children.Add(subLoadStack);
                trainingLoadStack.Children.Add(loadStack);
            }

            trainingPicker.SelectedIndexChanged += (sender, args) =>
            {
                trainingLoadStack.Children.Clear();
                var loadList =
                    JsonConvert.DeserializeObject<LoadStructure>(((TrainingMasterModel)trainingPicker.SelectedItem)
                        .Load);
                foreach (var load in loadList.LoadList)
                {
                    var loadStack = new StackLayout();
                    loadStack.Children.Add(new Label
                    {
                        Text = load.LoadName,
                        FontAttributes = FontAttributes.Bold,
                        Margin = new Thickness(0, 0, 0, 0)
                    });

                    var subLoadStack = new StackLayout { Orientation = StackOrientation.Horizontal };
                    subLoadStack.Children.Add(new Entry { Keyboard = Keyboard.Numeric, WidthRequest = 145 });
                    subLoadStack.Children.Add(new Picker
                    {
                        ItemsSource = LoadUnitService.GetLoadUnitList(load.Id),
                        ItemDisplayBinding = new Binding("UnitName"),
                        SelectedIndex = 0,
                        WidthRequest = 145
                    });
                    loadStack.Children.Add(subLoadStack);
                    trainingLoadStack.Children.Add(loadStack);
                }
            };

            mainStack.Children.Add(trainingLoadStack);
            TrainingStack.Children.Add(mainStack);
        }

        public void CommandDeleteTrainingAction()
        {
            if (TrainingStack.Children.Count != 1) TrainingStack.Children.RemoveAt(TrainingStack.Children.Count - 1);
        }

        public void CommandCancelAction()
        {
            // 遷移元画面をリロードする
            ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingPrevPageReload);
            ViewModelConst.TrainingPageNavigation.PopAsync();
        }

        private async Task CommandSaveAction()
        {
            try
            {
                if (!Validate())
                {
                    ViewModelCommonUtil.SendMessage(ViewModelConst.MessagingTrainingSelfScroll);
                    return;
                }

                IsLoading = true;
                var trainingContentList = new List<TrainingListStructure>();

                if (!Off)
                {
                    // トレーニング一覧
                    var trainingStack = TrainingStack.Children;
                    foreach (var training in trainingStack)
                    {
                        var insert = new TrainingListStructure();
                        var trainingId =
                            ((TrainingMasterModel)((Picker)((StackLayout)training).Children[1]).SelectedItem).Id;
                        var trainingseCount = ((Entry)((StackLayout)training).Children[3]).Text;

                        var loadContentList = new List<LoadContentStructure>();
                        var loadStack = ((StackLayout)((StackLayout)training).Children[4]).Children;
                        foreach (var load in loadStack)
                        {
                            var insertload = new LoadContentStructure();
                            var subLoad = ((StackLayout)load).Children[1];
                            var loadId = ((LoadUnitModel)((Picker)((StackLayout)subLoad).Children[1]).SelectedItem)
                                .LoadId;
                            var nums = ((Entry)((StackLayout)subLoad).Children[0]).Text;
                            var loadUnitId =
                                ((LoadUnitModel)((Picker)((StackLayout)subLoad).Children[1]).SelectedItem).Id;
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
                }

                var trainingScheduleStructure =
                    new TrainingScheduleStructure
                    {
                        TrainingContentList = trainingContentList,
                        Off = Off,
                        Week = _week
                    };

                var trainingScheduleStructureJson = JsonConvert.SerializeObject(trainingScheduleStructure);

                if (_isUpdate)
                    TrainingScheduleService.UpdateTrainingSchedule(_id, trainingScheduleStructureJson,
                        _week);
                else
                    TrainingScheduleService.RegistTrainingSchedule(trainingScheduleStructureJson,
                        _week);

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

        #endregion Command Actions
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Init Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Init Commands

        private void InitCommands()
        {
            CommandAddTraining = new Command(CommandAddTrainingAction);
            CommandDeleteTraining = new Command(CommandDeleteTrainingAction);
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

        private bool Validate()
        {
            ErrorStack.Clear();

            if (Off) return true;

            var trainingStack = TrainingStack.Children;
            foreach (var training in trainingStack)
            {
                var trainingseCount = ((Entry)((StackLayout)training).Children[3]).Text;
                if (StringUtils.IsEmpty(trainingseCount))
                {
                    ErrorStack.Add(CreateErrorLabel(LanguageKeys.SetCount, LanguageKeys.NotInputRequireData));
                }
                else
                {
                    if (!int.TryParse(trainingseCount, out var _))
                        ErrorStack.Add(CreateErrorLabel(LanguageKeys.SetCount, LanguageKeys.NotAvailableDataInput));
                }

                var loadStack = ((StackLayout)((StackLayout)training).Children[4]).Children;
                foreach (var load in loadStack)
                {
                    var subLoad = ((StackLayout)load).Children[1];
                    var nums = ((Entry)((StackLayout)subLoad).Children[0]).Text;
                    if (StringUtils.IsEmpty(nums))
                    {
                        ErrorStack.Add(CreateErrorLabel(LanguageKeys.LoadNum, LanguageKeys.NotInputRequireData));
                    }
                    else
                    {
                        if (!float.TryParse(nums, out var _))
                            ErrorStack.Add(CreateErrorLabel(LanguageKeys.LoadNum, LanguageKeys.NotAvailableDataInput));
                    }
                }

                if (ErrorStack.Count != 0) return false;
            }

            return true;
        }

        /// <summary>
        ///     エラーラベルを生成します
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