using HealthManager.Model.Service.Implement;
using HealthManager.Model.Service.Interface;
using HealthManager.ViewModel;
using Unity;

namespace HealthManager.Common.Other
{
    internal class ContainerInitializer
    {
       internal static UnityContainer InitializeContainer()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterInstance(typeof(InputBasicDataViewModel));
            unityContainer.RegisterInstance(typeof(RegistBodyImageViewModel));
            unityContainer.RegisterInstance(typeof(BodyImageViewModel));
            unityContainer.RegisterInstance(typeof(DataChartViewModel));
            unityContainer.RegisterInstance(typeof(DataSelectViewModel));
            unityContainer.RegisterInstance(typeof(EditTrainingScheduleViewModel));
            unityContainer.RegisterInstance(typeof(HomeViewModel));
            unityContainer.RegisterInstance(typeof(InputBasicDataViewModel));
            unityContainer.RegisterInstance(typeof(NewsWebViewModel));
            unityContainer.RegisterInstance(typeof(RegistBodyImageViewModel));
            unityContainer.RegisterInstance(typeof(TrainingHomeViewModel));
            unityContainer.RegisterInstance(typeof(TrainingListViewModel));
            unityContainer.RegisterInstance(typeof(TrainingMasterViewModel));
            unityContainer.RegisterInstance(typeof(TrainingReportListViewModel));
            unityContainer.RegisterInstance(typeof(TrainingReportViewModel));
            unityContainer.RegisterInstance(typeof(TrainingScheduleListViewModel));
            unityContainer.RegisterInstance(typeof(TrainingViewModel));

            unityContainer.RegisterType<IBasicDataService, BasicDataService>();
            unityContainer.RegisterType<IBodyImageService, BodyImageService>();
            unityContainer.RegisterType<ILoadService, LoadService>();
            unityContainer.RegisterType<ILoadUnitService, LoadUnitService>();
            unityContainer.RegisterType<ILocationService, LocationService>();
            unityContainer.RegisterType<IPartService, PartService>();
            unityContainer.RegisterType<ISubPartService, SubPartService>();
            unityContainer.RegisterType<ITrainingMasterService, TrainingMasterService>();
            unityContainer.RegisterType<ITrainingMenuService, TrainingMenuService>();
            unityContainer.RegisterType<ITrainingResultService, TrainingResultService>();
            unityContainer.RegisterType<ITrainingScheduleService, TrainingScheduleService>();

            return unityContainer;
        }
    }
}