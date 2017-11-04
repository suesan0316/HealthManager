using System;
using HealthManager.DependencyInterface;
using HealthManager.iOS.DependencyImplement;
using Xamarin.Forms;

[assembly: Dependency(typeof(SqliteDeviceInformImplementation))]
namespace HealthManager.iOS.DependencyImplement
{
   public class SqliteDeviceInformImplementation : ISqliteDeviceInform
    {
        public string GetDbPath()
        {
            var docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libFolder = System.IO.Path.Combine(docFolder, "..", "Library", "Databases");

            if (!System.IO.Directory.Exists(libFolder))
            {
                System.IO.Directory.CreateDirectory(libFolder);
            }

            return System.IO.Path.Combine(libFolder, "healthmanger.db3");
        }
    }
}