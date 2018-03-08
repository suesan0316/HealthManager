﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common.Constant;
using HealthManager.Common.Extention;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Model.Structure;
using HealthManager.Properties;
using HealthManager.View;
using HealthManager.ViewModel.Logic.News.Factory;
using HealthManager.ViewModel.Structure;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングホーム画面VMクラス
    /// </summary>
    public class TrainingHomeViewModel : INotifyPropertyChanged
    {

        /// <summary>
        ///     読み込み中フラグ
        /// </summary>
        private bool _isLoading;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public TrainingHomeViewModel()
        {
            EditTrainingScheduleCommand = new Command(MoveToTrainingSchedule);
            StartTrainingCommand = new Command(MoveToTraining);
            EditTrainingCommand = new Command(MoveToTrainingList);
            TrainingReportCommand = new Command(MoveToTrainingReport);

            NewsListItemTappedCommand = new Command<NewsStructure>(item =>
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new NewsWebView(item.NewsUrl,ViewModelConst.TrainingPageNavigation));
            });

            Task.Run(SetNewsSourceTask);
        }

        /// <summary>
        ///     トレーニングを編集するコマンド
        /// </summary>
        public ICommand EditTrainingCommand { get; set; }

        /// <summary>
        ///     トレーニングスケジュールを編集するコマンド
        /// </summary>
        public ICommand EditTrainingScheduleCommand { get; set; }

        /// <summary>
        ///     トレーニングを開始するコマンド
        /// </summary>
        public ICommand StartTrainingCommand { get; set; }

        /// <summary>
        ///     ニュース一覧アイテムタップコマンド
        /// </summary>
        public ICommand NewsListItemTappedCommand { get; set; }

        public ICommand TrainingReportCommand { get; set; }

        /// <summary>
        ///     ニュース一覧のアイテムリスト
        /// </summary>
        public ObservableCollection<NewsStructure> Items { set; get; } =
            new ObservableCollection<NewsStructure>();

        /// <summary>
        ///     トレーニングスケジュールを編集するボタンラベル
        /// </summary>
        public string EditTrainingScheduleLabel => LanguageUtils.Get(LanguageKeys.EditTrainingSchedule);

        /// <summary>
        ///     トレーニングスケジュールを編集するボタンラベル
        /// </summary>
        public string EditTrainingLabel => LanguageUtils.Get(LanguageKeys.EditTraining);

        /// <summary>
        ///     トレーニングを開始するボタンラベル
        /// </summary>
        public string StrartTrainingLabel => LanguageUtils.Get(LanguageKeys.StartTraining);

        /// <summary>
        /// トレーニングリストのボタンイメージ
        /// </summary>
        public string TrainingListButtonImage => ViewModelConst.TrainingListImage;

        /// <summary>
        /// トレーニングスケジュールのボタンイメージ
        /// </summary>
        public string TrainingScheduleButtonImage => ViewModelConst.TrainingScheduleImage;

        /// <summary>
        /// トレーニングスタートのボタンイメージ
        /// </summary>
        public string TrainingStartButtonImage => ViewModelConst.TrainingStartImage;

        public string TrainingReportLabel => LanguageUtils.Get(LanguageKeys.ConfirmTrainingReport);

        /// <summary>
        /// トレーニングニュース一覧のラベル
        /// </summary>
        public string TrainingNewsListTitleLabel => LanguageUtils.Get(LanguageKeys.TrainingNewsListTitle);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     読み込み中フラグ
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        /// <summary>
        ///     ニュース一覧を取得
        /// </summary>
        /// <returns></returns>
        private async Task SetNewsSourceTask()
        {
            IsLoading = true;
            var service = NewsServiceFactory.CreateNewsService();
            var structures = await service.GetTrainingNewsData();
            //structures.ForEach(data => Items.Add(data));
            Items = new ObservableCollection<NewsStructure>(structures);
            OnPropertyChanged(nameof(Items));
            IsLoading = false;
        }

        /// <summary>
        /// トレーニング一覧画面遷移
        /// </summary>
        private static void MoveToTrainingList()
        {
            ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingListView());
        }

        /// <summary>
        ///     トレーニングスケジュール画面遷移
        /// </summary>
        private static void MoveToTrainingSchedule()
        {
            ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingScheduleListView());
        }

        private static void MoveToTrainingReport()
        {
            var check = TrainingResultService.GeTrainingResultDataList();

            if (check.Count == 0)
            {
                Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.NotExistTrainingReport), LanguageUtils.Get(LanguageKeys.OK));
            }
            else
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingReportListView());
            }
        }

        /// <summary>
        ///     トレーニング画面遷移
        /// </summary>
        private static void MoveToTraining()
        {
            var check = TrainingResultService.CheckExitTargetDayData(DateTime.Now);

            if (check)
            {
                Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.TodayTrainingAlreadyCompleted), LanguageUtils.Get(LanguageKeys.OK));
            }
            else
            {
                var exits = TrainingScheduleService.GetTrainingSchedule((int) DateTime.Now.DayOfWeek);

                if (exits == null)
                {
                    Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                        LanguageUtils.Get(LanguageKeys.NotSettingTrainingSchedule), LanguageUtils.Get(LanguageKeys.OK));
                }
                else
                {

                    var training = JsonConvert
                        .DeserializeObject<TrainingScheduleStructure>(
                            TrainingScheduleService.GetTrainingSchedule((int) DateTime.Now.DayOfWeek).TrainingMenu);

                    if (training.Off)
                    {
                        Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                            LanguageUtils.Get(LanguageKeys.TodayIsRest), LanguageUtils.Get(LanguageKeys.OK));
                    }
                    else
                    {
                        ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingView());
                    }
                }
            }           
        }
    }
}