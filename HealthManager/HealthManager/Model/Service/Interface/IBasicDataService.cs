using System;
using System.Collections.Generic;

namespace HealthManager.Model.Service.Interface
{
    public interface IBasicDataService
    {
        bool RegistBasicData(string name, int locationId, int gender, DateTime birthday, float height,
            float bodyWeight,
            float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure, int basalMetabolism);

        bool UpdateBasicData(int id, string name, int locationId, int gender, DateTime birthday, float height,
            float bodyWeight, float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure, int basalMetabolism);

        BasicDataModel GetBasicData();

        List<BasicDataModel> GetBasicDataList();

        bool CheckExitTargetDayData(DateTime targeTime);
    }
}