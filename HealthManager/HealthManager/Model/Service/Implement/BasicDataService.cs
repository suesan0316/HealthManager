using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using HealthManager.Model.Service.Interface;
using SQLite;

namespace HealthManager.Model.Service.Implement
{
    /// <summary>
    ///     基本情報サービスクラス
    /// </summary>
    public class BasicDataService : IBasicDataService
    {
        /// <summary>
        ///     基本情報登録
        /// </summary>
        /// <param name="name"></param>
        /// <param name="locationId"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="height"></param>
        /// <param name="bodyWeight"></param>
        /// <param name="bodyFatPercentage"></param>
        /// <param name="maxBloodPressure"></param>
        /// <param name="minBloodPressure"></param>
        /// <param name="basalMetabolism"></param>
        /// <returns></returns>
        public bool RegistBasicData(string name, int locationId, int gender, DateTime birthday, float height,
            float bodyWeight,
            float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure, int basalMetabolism)
        {
            var model = new BasicDataModel
            {
                Name = name,
                LocationId = locationId,
                Gender = gender,
                BirthDay = birthday,
                Height = height,
                BodyWeight = bodyWeight,
                BodyFatPercentage = bodyFatPercentage,
                MaxBloodPressure = maxBloodPressure,
                MinBloodPressure = minBloodPressure,
                BasalMetabolism = basalMetabolism,
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

        /// <summary>
        ///     基本情報更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="locationId"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="height"></param>
        /// <param name="bodyWeight"></param>
        /// <param name="bodyFatPercentage"></param>
        /// <param name="maxBloodPressure"></param>
        /// <param name="minBloodPressure"></param>
        /// <param name="basalMetabolism"></param>
        /// <returns></returns>
        public bool UpdateBasicData(int id, string name, int locationId, int gender, DateTime birthday, float height,
            float bodyWeight, float bodyFatPercentage,
            int maxBloodPressure, int minBloodPressure, int basalMetabolism)
        {
            var model = new BasicDataModel
            {
                Id = id,
                Name = name,
                LocationId = locationId,
                Gender = gender,
                BirthDay = birthday,
                Height = height,
                BodyWeight = bodyWeight,
                BodyFatPercentage = bodyFatPercentage,
                MaxBloodPressure = maxBloodPressure,
                MinBloodPressure = minBloodPressure,
                BasalMetabolism = basalMetabolism,
                RegistedDate = DateTime.Now
            };
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = db.Update(model);
                if (result == DbConst.Failed) return false;
                db.Commit();
                return true;
            }
        }

        /// <summary>
        ///     基本情報取得
        /// </summary>
        /// <returns>最新日付の基本情報</returns>
        public BasicDataModel GetBasicData()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<BasicDataModel>()
                    orderby record.RegistedDate descending
                    select record;
                return result.Any() ? result.First() : null;
            }
        }

        /// <summary>
        ///     基本情報取得
        /// </summary>
        /// <returns>登録日時でソートした基本情報全件</returns>
        public List<BasicDataModel> GetBasicDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<BasicDataModel>() orderby record.RegistedDate select record;
                return result.Any() ? result.ToList() : new List<BasicDataModel>();
            }
        }

        /// <summary>
        ///     指定日の基本情報取得存在チェック
        /// </summary>
        /// <param name="targeTime"></param>
        /// <returns></returns>
        public bool CheckExitTargetDayData(DateTime targeTime)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<BasicDataModel>() select record;
                return result.Any() && result.ToList().Any(data => data.RegistedDate.Date == targeTime.Date);
            }
        }
    }
}