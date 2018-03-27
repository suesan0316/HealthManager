using System;
using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface ITrainingResultService
    {
        bool RegistTrainingResult(string trainingContent, string weather, DateTime targetDate, DateTime startDate,
            DateTime endDate);

        List<TrainingResultModel> GeTrainingResultDataList();
        bool CheckExitTargetDayData(DateTime targeTime);
    }
}