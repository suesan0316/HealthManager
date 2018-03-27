using CommonServiceLocator;

namespace HealthManager.ViewModel.Locator
{
    public class ViewModelLocator
    {
        public InputBasicDataViewModel InputBasicDataViewModel =>
            ServiceLocator.Current.GetInstance<InputBasicDataViewModel>();

        public RegistBodyImageViewModel RegistBodyImageViewModel =>
            ServiceLocator.Current.GetInstance<RegistBodyImageViewModel>();

        public DataChartViewModel DataChartViewModel => ServiceLocator.Current.GetInstance<DataChartViewModel>();
        public DataSelectViewModel DataSelectViewModel => ServiceLocator.Current.GetInstance<DataSelectViewModel>();

        public EditTrainingScheduleViewModel EditTrainingScheduleViewModel =>
            ServiceLocator.Current.GetInstance<EditTrainingScheduleViewModel>();

        public HomeViewModel HomeViewModel => ServiceLocator.Current.GetInstance<HomeViewModel>();
        public NewsWebViewModel NewsWebViewModel => ServiceLocator.Current.GetInstance<NewsWebViewModel>();

        public TrainingHomeViewModel TrainingHomeViewModel =>
            ServiceLocator.Current.GetInstance<TrainingHomeViewModel>();

        public TrainingListViewModel TrainingListViewModel =>
            ServiceLocator.Current.GetInstance<TrainingListViewModel>();

        public TrainingMasterViewModel TrainingMasterViewModel =>
            ServiceLocator.Current.GetInstance<TrainingMasterViewModel>();

        public TrainingReportListViewModel TrainingReportListViewModel =>
            ServiceLocator.Current.GetInstance<TrainingReportListViewModel>();

        public TrainingReportViewModel TrainingReportViewModel =>
            ServiceLocator.Current.GetInstance<TrainingReportViewModel>();

        public TrainingScheduleListViewModel TrainingScheduleListViewModel =>
            ServiceLocator.Current.GetInstance<TrainingScheduleListViewModel>();

        public TrainingViewModel TrainingViewModel => ServiceLocator.Current.GetInstance<TrainingViewModel>();
    }
}