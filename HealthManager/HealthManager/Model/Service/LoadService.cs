using System;
using System.Collections.Generic;
using System.Linq;
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
        /// IDでソートした負荷データを全件取得
        /// </summary>
        /// <returns></returns>
        public static List<LoadModel> GetLoadDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<LoadModel>() orderby record.Id select record;
                return result.Any() ? result.ToList() : new List<LoadModel>();
            }
        }

        public static LoadModel GetLoad(int id)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<LoadModel>() where record.Id == id select record;
                return result.Any() ? result.First() : null;
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
