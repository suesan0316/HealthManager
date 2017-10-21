using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common;
using SQLite;

namespace HealthManager.Model.Service
{
    public class BodyImageService
    {
        public static bool RegistBodyImage(string base64String)
        {
            var model = new BodyImageModel
            {
                ImageBase64String = base64String,
                RegistedDate = DateTime.Now
            };
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                db.Insert(model);
                db.Commit();
            }
            return true;
        }

        public static bool UpdateBodyImage(int id, string base64String)
        {
            var model = new BodyImageModel
            {
                Id = id,
                ImageBase64String = base64String,
                RegistedDate = DateTime.Now
            };
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                db.Update(model);
                db.Commit();
            }
            return true;
        }

        public static BodyImageModel GetBodyImage()
        {
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                var result = from record in db.Table<BodyImageModel>()
                    orderby record.RegistedDate descending
                    select record;
                return result.Any() ? result.First() : null;
            }
        }

        public static List<BodyImageModel> GetBodyImageList()
        {
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                var result = from record in db.Table<BodyImageModel>() orderby record.RegistedDate select record;
                return result.Any() ? result.ToList() : new List<BodyImageModel>();
            }
        }

        public static bool CheckExitTargetDayData(DateTime targeTime)
        {
            using (var db = new SQLiteConnection(DbConst.DBPath))
            {
                var result = from record in db.Table<BodyImageModel>() select  record;

                return result.Any() && result.ToList().Any(data => data.RegistedDate.Date == targeTime.Date);
            }
        }
    }
}