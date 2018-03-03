using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using SQLite;

namespace HealthManager.Model.Service
{
   public class LoadUnitService
    {
        public static List<LoadUnitModel> GetLoadUnitList(int loadId)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<LoadUnitModel>() where record.LoadId == loadId orderby record.LoadId select record;
                return result.Any() ? result.ToList() : new List<LoadUnitModel>();
            }
        }

        public static LoadUnitModel GetLoadUnit(int id)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<LoadUnitModel>() where record.Id == id orderby record.LoadId select record;
                return result.Any() ? result.First(): new LoadUnitModel();
            }
        }
    }
}
