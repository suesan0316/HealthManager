using System;
using System.Linq;
using HealthManager.Common.Constant;
using HealthManager.Model.Service.Interface;
using SQLite;

namespace HealthManager.Model.Service.Implement
{
    public class TrainingScheduleService : ITrainingScheduleService
    {
        /// <summary>
        ///     トレーニングスケジュール登録
        /// </summary>
        /// <param name="trainingMenu"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public bool RegistTrainingSchedule(string trainingMenu, int week)
        {
            var model = new TrainingScheduleModel
            {
                TrainingMenu = trainingMenu,
                Week = week,
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

        public TrainingScheduleModel GetTrainingSchedule(int week)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<TrainingScheduleModel>() where record.Week == week select record;
                return result.Any() ? result.First() : null;
            }
        }

        /// <summary>
        ///     トレーニングスケジュール更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trainingMenu"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public bool UpdateTrainingSchedule(int id, string trainingMenu, int week)
        {
            var model = new TrainingScheduleModel
            {
                Id = id,
                TrainingMenu = trainingMenu,
                Week = week
            };
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = db.Update(model);
                if (result == DbConst.Failed) return false;
                db.Commit();
                return true;
            }
        }
    }
}