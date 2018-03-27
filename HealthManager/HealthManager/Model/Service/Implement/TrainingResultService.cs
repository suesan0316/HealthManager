using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using HealthManager.Model.Service.Interface;
using SQLite;

namespace HealthManager.Model.Service.Implement
{
    public class TrainingResultService : ITrainingResultService
    {
        public bool RegistTrainingResult(string trainingContent, string weather, DateTime targetDate,
            DateTime startDate, DateTime endDate)
        {
            var model = new TrainingResultModel
            {
                TrainingContent = trainingContent,
                Weather = weather,
                TargetDate = targetDate,
                StartDate = startDate,
                EndDate = endDate,
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

        public List<TrainingResultModel> GeTrainingResultDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<TrainingResultModel>()
                    orderby record.Id
                    select record;
                return result.Any() ? result.ToList() : new List<TrainingResultModel>();
            }
        }

        /// <summary>
        ///     指定日のトレーニング結果存在チェック
        /// </summary>
        /// <param name="targeTime"></param>
        /// <returns></returns>
        public bool CheckExitTargetDayData(DateTime targeTime)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<TrainingResultModel>() select record;
                return result.Any() && result.ToList().Any(data => data.TargetDate.Date == targeTime.Date);
            }
        }
    }
}