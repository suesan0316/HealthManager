using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthManager.Common.Constant;
using HealthManager.Common.Language;
using HealthManager.Model.Service;
using HealthManager.Model.Structure;
using HealthManager.View;
using HealthManager.ViewModel.Logic.News.Factory;
using HealthManager.ViewModel.Structure;
using Newtonsoft.Json;
using PropertyChanged;
using Xamarin.Forms;

namespace HealthManager.ViewModel
{
    /// <summary>
    ///     トレーニングホーム画面VMクラス
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class TrainingHomeViewModel
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

        #endregion Instance Private Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Constractor
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Constractor

        public TrainingHomeViewModel()
        {          
            InitCommands();
            Task.Run(SetNewsSourceTask);
        }

        #endregion Constractor
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Variables
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Variables

        /// <summary>
        ///     ニュース一覧のアイテムリスト
        /// </summary>
        public ObservableCollection<NewsStructure> Items { set; get; } =
            new ObservableCollection<NewsStructure>();

        /// <summary>
        ///     トレーニングリストのボタンイメージ
        /// </summary>
        public string TrainingListButtonImage => ViewModelConst.TrainingListImage;

        /// <summary>
        ///     トレーニングスケジュールのボタンイメージ
        /// </summary>
        public string TrainingScheduleButtonImage => ViewModelConst.TrainingScheduleImage;

        /// <summary>
        ///     トレーニングスタートのボタンイメージ
        /// </summary>
        public string TrainingStartButtonImage => ViewModelConst.TrainingStartImage;

        /// <summary>
        ///     読み込み中フラグ
        /// </summary>
        public bool IsLoading { get; set; }

        #endregion Binding Variables
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding DisplayLabels
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding DisplayLabels

        public string DisplayLabelEditTraining => LanguageUtils.Get(LanguageKeys.EditTraining);
        public string DisplayLabelEditTrainingSchedule => LanguageUtils.Get(LanguageKeys.EditTrainingSchedule);
        public string DisplayLabelStrartTraining => LanguageUtils.Get(LanguageKeys.StartTraining);
        public string DisplayLabelTrainingNewsListTitle => LanguageUtils.Get(LanguageKeys.TrainingNewsListTitle);
        public string DisplayLabelTrainingReport => LanguageUtils.Get(LanguageKeys.ConfirmTrainingReport);

        #endregion Binding DisplayLabels
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Binding Commands
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Binding Commands

        public ICommand CommandEditTraining { get; set; }
        public ICommand CommandEditTrainingSchedule { get; set; }
        public ICommand CommandNewsListItemTapped { get; set; }
        public ICommand CommandStartTraining { get; set; }
        public ICommand CommandTrainingReport { get; set; }

        #endregion Binding Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // Command Actions
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region Command Actions

        /// <summary>
        ///     トレーニング一覧画面遷移
        /// </summary>
        private static void CommandEditTrainingAction()
        {
            var check = TrainingMasterService.GetTrainingMasterDataList();

            if (check == null || check.Count == 0)
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingMasterView());
            }
            else
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingListView());
            }
        }

        /// <summary>
        ///     トレーニングスケジュール画面遷移
        /// </summary>
        private static void CommandEditTrainingScheduleAction()
        {
            var check = TrainingMasterService.GetTrainingMasterDataList();

            if (check == null || check.Count == 0)
            {
                Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.NotExistTraining), LanguageUtils.Get(LanguageKeys.OK));
                return;
            }

            ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingScheduleListView());
        }

        private static void CommandTrainingReportAction()
        {
            var check = TrainingResultService.GeTrainingResultDataList();

            if (check.Count == 0)
                Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.NotExistTrainingReport), LanguageUtils.Get(LanguageKeys.OK));
            else
                ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingReportListView());
        }

        /// <summary>
        ///     トレーニング画面遷移
        /// </summary>
        private static void CommandStartTrainingAction()
        {
            var check = TrainingResultService.CheckExitTargetDayData(DateTime.Now);

            if (check)
            {
                Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                    LanguageUtils.Get(LanguageKeys.TodayTrainingAlreadyCompleted), LanguageUtils.Get(LanguageKeys.OK));
            }
            else
            {
                var exits = TrainingScheduleService.GetTrainingSchedule((int)DateTime.Now.DayOfWeek);

                if (exits == null)
                {
                    Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                        LanguageUtils.Get(LanguageKeys.NotSettingTrainingSchedule), LanguageUtils.Get(LanguageKeys.OK));
                }
                else
                {
                    var training = JsonConvert
                        .DeserializeObject<TrainingScheduleStructure>(
                            TrainingScheduleService.GetTrainingSchedule((int)DateTime.Now.DayOfWeek).TrainingMenu);

                    if (training.Off)
                        Application.Current.MainPage.DisplayAlert(LanguageUtils.Get(LanguageKeys.Confirm),
                            LanguageUtils.Get(LanguageKeys.TodayIsRest), LanguageUtils.Get(LanguageKeys.OK));
                    else
                        ViewModelConst.TrainingPageNavigation.PushAsync(new TrainingView());
                }
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
            CommandEditTrainingSchedule = new Command(CommandEditTrainingScheduleAction);
            CommandStartTraining = new Command(CommandStartTrainingAction);
            CommandEditTraining = new Command(CommandEditTrainingAction);
            CommandTrainingReport = new Command(CommandTrainingReportAction);

            CommandNewsListItemTapped = new Command<NewsStructure>(item =>
            {
                ViewModelConst.TrainingPageNavigation.PushAsync(new NewsWebView(item.NewsUrl,
                    ViewModelConst.TrainingPageNavigation));
            });
        }

        #endregion Init Commands
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        //
        // ViewModel Logic
        //
        /*----------------------------------------------------------------------------------------------------------------------------------------*/
        #region ViewModel Logic

        /// <summary>
        ///     ニュース一覧を取得
        /// </summary>
        /// <returns></returns>
        private async Task SetNewsSourceTask()
        {
            IsLoading = true;
            var service = NewsServiceFactory.CreateNewsService();
            var structures = await service.GetTrainingNewsData();
            //structures.ForEach(data => RecivedRequest.Add(data));
            Items = new ObservableCollection<NewsStructure>(structures);
            IsLoading = false;
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