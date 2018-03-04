using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HealthManager.Annotations;
using HealthManager.Common;
using HealthManager.Common.Constant;
using HealthManager.Common.Enum;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Model.Structure;
using HealthManager.View;
using HealthManager.ViewModel.Structure;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングスケジュール一覧画面VMクラス
    /// </summary>
    public class TrainingScheduleListViewModel : INotifyPropertyChanged
    {
        private readonly List<WeekEnum> weekList;

        public TrainingScheduleListViewModel()
        {
            BackPageCommand = new Command(ViewModelCommonUtil.TrainingBackPage);
            TrainingScheduleListItemTappedCommand = new Command<TrainingScheduleSViewtructure>(item =>
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new EditTrainingScheduleView(item.Week));
            });

            weekList = new List<WeekEnum>();
            foreach (var week in Enum.GetValues(typeof(WeekEnum)))
            {
                weekList.Add((WeekEnum) week);
               Items.Add(ViewModelCommonUtil.CreateTrainingScheduleSViewtructure((WeekEnum)week));
               // CreateTrainingScheduleListItem((WeekEnum) week);
            }
        }

        public ObservableCollection<TrainingScheduleSViewtructure> Items { protected set; get; } =
            new ObservableCollection<TrainingScheduleSViewtructure>();

        /// <summary>
        ///     戻るボタンコマンド
        /// </summary>
        public ICommand BackPageCommand { get; set; }

        public ICommand TrainingScheduleListItemTappedCommand { get; set; }

        /// <summary>
        ///     戻るボタンラベル
        /// </summary>
        public string BackPageLabel => LanguageUtils.Get(LanguageKeys.Return);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // TODO 後で消す
        private void CreateTrainingScheduleListItem(WeekEnum week)
        {

            var model = TrainingScheduleService.GetTrainingSchedule((int) week);

            if (model == null)
            {
                var empty = new TrainingScheduleSViewtructure
                {
                    Week = (int)week,
                    WeekName = week.ToString(),
                    Off = false
                };
                Items.Add(empty);
                return;
            }

            var trainingScheduleStructure =
                JsonConvert.DeserializeObject<TrainingScheduleStructure>(model.TrainingMenu);

            var trainingScheduleViewStructure = new TrainingScheduleSViewtructure
            {
                Week = (int) week,
                WeekName = week.ToString(),
                Off = trainingScheduleStructure.Off
            };

            var trainingListViewStructureList = new List<TrainingListViewStructure>();

            int count = 1;
            foreach (var training in trainingScheduleStructure.TrainingContentList)
            {
                var trainingListViewStructure = new TrainingListViewStructure
                {
                    TrainingId = training.TrainingId,
                    TrainingNo = count,
                    TrainingName = TrainingMasterService.GetTrainingMasterData(training.TrainingId).TrainingName,
                    TrainingSetCount = training.TrainingSetCount
                };
                var loadContentViewStructureList = new List<LoadContentViewStructure>();

                foreach (var load in training.LoadContentList)
                {
                    var loadContentViewStructure = new LoadContentViewStructure
                    {
                        LoadId = load.LoadId,
                        LoadName = LoadService.GetLoad(load.LoadId).LoadName,
                        Nums =load.Nums.ToString(),
                        LoadUnitId = load.LoadUnitId,
                        LoadUnitName = LoadUnitService.GetLoadUnit(load.LoadUnitId).UnitName
                    };
                    loadContentViewStructureList.Add(loadContentViewStructure);
                }

                trainingListViewStructure.LoadContentList = loadContentViewStructureList;

                trainingListViewStructureList.Add(trainingListViewStructure);
                count++;
            }

            trainingScheduleViewStructure.TrainingContentList = trainingListViewStructureList;

            Items.Add(trainingScheduleViewStructure);
        }
    }
}