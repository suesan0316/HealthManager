using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using Newtonsoft.Json;
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
        /// <param name="id"></param>
        /// <param name="trainingName"></param>
        /// <param name="load"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public static bool UpdateTrainingMaster(int id, string trainingName, string load, string part)
        {
            var model = new TrainingMasterModel
            {
                Id = id,
                TrainingName = trainingName,
                Load = load,
                Part = part,
                RegistedDate = DateTime.Now
            };
            var s = JsonConvert.SerializeObject(model);
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

        /// <summary>
        /// トレーニングマスタ取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TrainingMasterModel GetTrainingMasterData(int id)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<TrainingMasterModel>()
                             where record.Id == id
                    select record;
                return result.Any() ? result.First() : null;
            }
        }

        /// <summary>
        /// トレーニングマスタ全件取得
        /// </summary>
        /// <returns></returns>
        public static List<TrainingMasterModel> GetTrainingMasterDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<TrainingMasterModel>()
                    orderby record.Id
                    select record;
                return result.Any() ? result.ToList() : null;
            }
        }
    }
}
