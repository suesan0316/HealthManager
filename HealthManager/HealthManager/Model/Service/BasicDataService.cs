using HealthManager.DependencyInterface;
using SQLite;
using Xamarin.Forms;

namespace HealthManager.Model.Service
{
    public class BasicDataService
    {
        public static bool RegistBasicData(string name, bool sex, int age, double height, double bodyWeight,
            int maxBloodPressure, int minBloodPressure)
        {
            var model = new BasicDataModel
            {
                Name = name,
                Sex = sex,
                Age = age,
                Height = height,
                BodyWeight = bodyWeight,
                MaxBloodPressure = maxBloodPressure,
                MinBloodPressure = minBloodPressure
            };
            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());
            db.Insert(model);
            return true;
        }
    }
}