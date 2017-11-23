using System;
using HealthManager.Common;
using SQLite;

namespace HealthManager.Model.Service
{
    /// <summary>
    /// トレーニングスケジュールサービスクラス
    /// </summary>
    public class TrainingScheduleService
    {
        /// <summary>
        /// トレーニングスケジュール登録
        /// </summary>
        /// <param name="trainingMenuId"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static bool RegistTrainingSchedule(int trainingMenuId, int week)
        {
            var model = new TrainingScheduleModel
            {
                TrainingMenuId = trainingMenuId,
                Week = week,
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

        /// <summary>
        /// トレーニングスケジュール更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trainingMenuId"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static bool UpdateTrainingSchedule(int id,int trainingMenuId, int week)
        {
            var model = new TrainingScheduleModel
            {
                Id = id,
                TrainingMenuId = trainingMenuId,
                Week = week,
            };
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = db.Update(model);
                if (result == DbConst.Failed)
                {
                    return false;
                }
                db.Commit();
                return true;
            }
        }
    }
}
