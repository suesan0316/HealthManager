using System;
using HealthManager.Common.Constant;
using SQLite;

namespace HealthManager.Model.Service
{
    /// <summary>
    /// トレーングマスタサービスクラス
    /// </summary>
    public class TrainingMasterService
    {

        /// <summary>
        /// トレーニングマスタ登録
        /// </summary>
        /// <param name="trainingName"></param>
        /// <param name="load"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public static bool RegistTrainingMaster(string trainingName, string load, string part)
        {
            var model = new TrainingMasterModel
            {
                TrainingName = trainingName,
                Load = load,
                Part = part,
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
        /// トレーニングマスタ更新
        /// </summary>
        /// <param name="trainingName"></param>
        /// <param name="load"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public static bool UpdateTrainingMaster(string trainingName, string load, string part)
        {
            var model = new TrainingMasterModel
            {
                TrainingName = trainingName,
                Load = load,
                Part = part,
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

    }
}
