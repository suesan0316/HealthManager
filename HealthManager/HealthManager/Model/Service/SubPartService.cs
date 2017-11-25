using System;
using HealthManager.Common;
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
