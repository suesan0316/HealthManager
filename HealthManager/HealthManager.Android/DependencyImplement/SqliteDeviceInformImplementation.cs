using HealthManager.DependencyInterface;
using HealthManager.Droid.DependencyImplement;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqliteDeviceInformImplementation))]
namespace HealthManager.Droid.DependencyImplement
{
    class SqliteDeviceInformImplementation : ISqliteDeviceInform
    {
        public string GetDbPath()
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return System.IO.Path.Combine(path, "healthmanger.db3");
        }
    }
}