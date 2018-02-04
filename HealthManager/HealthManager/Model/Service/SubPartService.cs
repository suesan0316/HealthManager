using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using SQLite;

namespace HealthManager.Model.Service
{
    /// <summary>
    /// サブ部位サービス
    /// </summary>
    public class SubPartService
    {
        /// <summary>
        /// サブ部位登録
        /// </summary>
        /// <param name="parentPartId"></param>
        /// <param name="subPartName"></param>
        /// <returns></returns>
        public static bool RegistSubPart(int parentPartId, string subPartName)
        {
            var model = new SubPartModel
            {
                ParentPartId = parentPartId,
                SubPartName =  subPartName,
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
        /// サブ部位名でソートしたサブ部位データを全件取得
        /// </summary>
        /// <returns></returns>
        public static List<SubPartModel> GetSubPartDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<SubPartModel>() orderby record.SubPartName select record;
                return result.Any() ? result.ToList() : new List<SubPartModel>();
            }
        }

        /// <summary>
        /// 親部位IDで絞ったサブ部位データを取得
        /// </summary>
        /// <param name="parentPartId"></param>
        /// <returns></returns>
        public static List<SubPartModel> GetSubPartDataList(int parentPartId)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<SubPartModel>() where record.ParentPartId == parentPartId orderby record.SubPartName select record;
                return result.Any() ? result.ToList() : new List<SubPartModel>();
            }
        }

        /// <summary>
        /// サブ部位更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentPartId"></param>
        /// <param name="subPartName"></param>
        /// <returns></returns>
        public static bool UpdateSubPart(int id,int parentPartId, string subPartName)
        {
            var model = new SubPartModel
            {
                Id = id,
                ParentPartId = parentPartId,
                SubPartName = subPartName
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
