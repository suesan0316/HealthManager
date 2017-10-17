using System.Collections.Generic;
using System.Linq;
using HealthManager.DependencyInterface;
using SQLite;
using Xamarin.Forms;

namespace HealthManager.Model.Service
{
    public class BasicDataService
    {
        public static bool RegistBasicData(string name, bool sex, int age, float height, float bodyWeight,float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure,int basalMetabolism)
        {
            var model = new BasicDataModel
            {
                Name = name,
                Sex = sex,
                Age = age,
                Height = height,
                BodyWeight = bodyWeight,
                BodyFatPercentage =bodyFatPercentage,
                MaxBloodPressure = maxBloodPressure,
                MinBloodPressure = minBloodPressure,
                BasalMetabolism = basalMetabolism,
                RegistedDate = System.DateTime.Now
            };
            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());
            db.Insert(model);
            return true;
        }

        public static BasicDataModel GetBasicData()
        {
            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());
            var result = from record in db.Table<BasicDataModel>() orderby record.RegistedDate descending select record;
            return result.First();
        }
        public static List<BasicDataModel> GetBasicDataList()
        {
            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());
            var result = from record in db.Table<BasicDataModel>() orderby record.RegistedDate  select record;
            return result.ToList();
        }
    }
}