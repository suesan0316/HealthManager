using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using HealthManager.Model.Service.Interface;
using SQLite;

namespace HealthManager.Model.Service.Implement
{
    public class LocationService : ILocationService
    {
        public bool RegistLocation(string locationName, string cityId)
        {
            var model = new LocationModel
            {
                LocationName = locationName,
                CityId = cityId,
                RegistedDate = DateTime.Now
            };
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = db.Insert(model);
                if (result == DbConst.Failed) return false;
                db.Commit();
                return true;
            }
        }

        public List<LocationModel> GetLoactionDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<LocationModel>()
                    orderby record.Id
                    select record;
                return result.Any() ? result.ToList() : null;
            }
        }

        public LocationModel GetLocation(int id)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<LocationModel>()
                    where record.Id == id
                    select record;
                return result.Any() ? result.First() : null;
            }
        }
    }
}