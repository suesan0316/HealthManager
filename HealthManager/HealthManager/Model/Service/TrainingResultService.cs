using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using SQLite;

namespace HealthManager.Model.Service
{
    public class TrainingResultService
    {
        public static bool RegistTrainingResult(string trainingContent,string weather,DateTime targetDate,DateTime startDate,DateTime endDate)
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
                if (result == DbConst.Failed)
                {
                    return false;
                }
                db.Commit();
                return true;
            }
        }

        public static List<TrainingResultModel> GeTrainingResultDataList()
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
        /// 指定日のトレーニング結果存在チェック
        /// </summary>
        /// <param name="targeTime"></param>
        /// <returns></returns>
        public static bool CheckExitTargetDayData(DateTime targeTime)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<TrainingResultModel>() select record;
                return result.Any() && result.ToList().Any(data => data.TargetDate.Date == targeTime.Date);
            }
        }
    }
}
