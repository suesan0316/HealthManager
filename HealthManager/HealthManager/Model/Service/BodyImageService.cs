using System.Linq;
using HealthManager.DependencyInterface;
using SQLite;
using Xamarin.Forms;

namespace HealthManager.Model.Service
{
    public class BodyImageService
    {
        public static bool RegistBodyImage(string base64String)
        {
            var model = new BodyImageModel
            {
                ImageBase64String = base64String,
                RegistedDate = System.DateTime.Now
            };
            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());
            db.Insert(model);
            db.Commit();
            return true;
        }

        public static BodyImageModel GetBodyImage()
        {
            var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath());
            var result = from record in db.Table<BodyImageModel>() orderby record.RegistedDate descending select record;
            return result.First();
        }
    }
}