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
using HealthManager.View;
using HealthManager.ViewModel.Structure;
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
            TrainingScheduleListItemTappedCommand = new Command(MoveToTrainingSchedule);

            weekList = new List<WeekEnum>();
            foreach (var week in Enum.GetValues(typeof(WeekEnum)))
            {
                weekList.Add((WeekEnum) week);
                CreateTrainingScheduleListItem((WeekEnum) week);
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

        public void MoveToTrainingSchedule()
        {
            ViewModelConst.TrainingPageNavigation.PushAsync(new EditTrainingScheduleView());
        }

        private void CreateTrainingScheduleListItem(WeekEnum week)
        {
            var trainingScheduleViewStructure = new TrainingScheduleSViewtructure
            {
                Week = (int) week,
                WeekName = week.ToString(),
                Off = false
            };

            var trainingListViewStructureList = new List<TrainingListViewStructure>();
            var trainingListViewStructure = new TrainingListViewStructure
            {
                TrainingId = 1,
                TrainingNo = 2,
                TrainingName = "腕立て伏せ",
                TrainingSetCount = 3
            };
            trainingListViewStructureList.Add(trainingListViewStructure);

            var loadContentViewStructureList = new List<LoadContentViewStructure>();
            var loadContentViewStructure = new LoadContentViewStructure
            {
                LoadId = 4,
                LoadName = "重量",
                Nums = "これぐらい"
            };
            loadContentViewStructureList.Add(loadContentViewStructure);
            trainingListViewStructure.LoadContentList = loadContentViewStructureList;

            trainingScheduleViewStructure.TrainingContentList = trainingListViewStructureList;

            Items.Add(trainingScheduleViewStructure);
        }
    }
}