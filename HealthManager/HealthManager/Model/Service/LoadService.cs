using System;
using HealthManager.Common.Constant;
using SQLite;

namespace HealthManager.Model.Service
{
    /// <summary>
    /// 負荷サービス
    /// </summary>
    public class LoadService
    {
        /// <summary>
        /// 負荷登録
        /// </summary>
        /// <param name="loadName"></param>
        /// <returns></returns>
        public static bool RegistLoad(string loadName)
        {
            var model = new LoadModel
            {
                LoadName = loadName,
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
        /// 負荷更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loadName"></param>
        /// <returns></returns>
        public static bool UpdateLoad(int id, string loadName)
        {
            var model = new LoadModel
            {
                Id = id,
                LoadName = loadName
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
