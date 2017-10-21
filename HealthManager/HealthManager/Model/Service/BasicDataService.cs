using System.Collections.Generic;
using System.Linq;
using HealthManager.Common;
using SQLite;

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
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                db.Insert(model);
                db.Commit();
            }
            return true;
        }

        public static bool UpdateBasicData(int id, string name, bool sex, int age, float height, float bodyWeight, float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure, int basalMetabolism)
        {
            var model = new BasicDataModel
            {
                Id = id,
                Name = name,
                Sex = sex,
                Age = age,
                Height = height,
                BodyWeight = bodyWeight,
                BodyFatPercentage = bodyFatPercentage,
                MaxBloodPressure = maxBloodPressure,
                MinBloodPressure = minBloodPressure,
                BasalMetabolism = basalMetabolism,
                RegistedDate = System.DateTime.Now
            };
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                db.Update(model);
                db.Commit();
            }
            return true;
        }

        public static BasicDataModel GetBasicData()
        {
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                var result = from record in db.Table<BasicDataModel>()
                    orderby record.RegistedDate descending
                    select record;
                return result.Count() != 0 ? result.First() : null;
            }
        }
        public static List<BasicDataModel> GetBasicDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                var result = from record in db.Table<BasicDataModel>() orderby record.RegistedDate select record;
                return result.Count() != 0 ? result.ToList() : new List<BasicDataModel>();
            }
        }
    }
}