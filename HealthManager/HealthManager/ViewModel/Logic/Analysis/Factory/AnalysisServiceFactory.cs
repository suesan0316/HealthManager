using System;
using CommonServiceLocator;
using HealthManager.Common.Enum;
using HealthManager.ViewModel.Logic.Analysis.Implement;
using HealthManager.ViewModel.Logic.Analysis.Service;

namespace HealthManager.ViewModel.Logic.Analysis.Factory
{
    public class AnalysisServiceFactory
    {
        public static IAnalysisService Create(BasicDataEnum basicDataEnum)
        {
            switch (basicDataEnum)
            {
                case BasicDataEnum.Name:
                    return null;
                case BasicDataEnum.Sex:
                    return null;
                case BasicDataEnum.Age:
                    return null;
                case BasicDataEnum.Height:
                    return null;
                case BasicDataEnum.BodyWeight:
                    return ServiceLocator.Current.GetInstance < BodyWeightAnalysisServiceImpl>();
                case BasicDataEnum.BodyFatPercentage:
                    return ServiceLocator.Current.GetInstance < BodyFatPercentageAnalysisServiceImpl>();
                case BasicDataEnum.MaxBloodPressure:
                    return ServiceLocator.Current.GetInstance < MaxBloodPressureAnalysisServiceImpl>();
                case BasicDataEnum.MinBloodPressure:
                    return ServiceLocator.Current.GetInstance < MinBloodPressureAnalysisServiceImpl>();
                case BasicDataEnum.BasalMetabolism:
                    return ServiceLocator.Current.GetInstance<BasalMetabolismAnalysisServiceImpl>();
                case BasicDataEnum.Location:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(basicDataEnum), basicDataEnum, null);
            }
        }
    }
}
