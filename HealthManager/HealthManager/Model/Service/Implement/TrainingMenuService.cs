using System;
using HealthManager.Common.Constant;
using HealthManager.Model.Service.Interface;
using SQLite;

namespace HealthManager.Model.Service.Implement
{
    public class TrainingMenuService : ITrainingMenuService
    {
        /// <summary>
        ///     トレーニングメニュー登録
        /// </summary>
        /// <param name="trainingId"></param>
        /// <param name="menuName"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        public bool RegistTrainingMenu(int trainingId, string menuName, string load)
        {
            var model = new TrainingMenuModel
            {
                TrainingId = trainingId,
                MenuName = menuName,
                Load = load,
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
        ///     トレーニングメニュー更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trainingId"></param>
        /// <param name="menuName"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        public bool UpdateTrainingMenu(int id, int trainingId, string menuName, string load)
        {
            var model = new TrainingMenuModel
            {
                Id = id,
                TrainingId = trainingId,
                MenuName = menuName,
                Load = load
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