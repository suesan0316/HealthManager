using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using HealthManager.Model.Service.Interface;
using SQLite;

namespace HealthManager.Model.Service.Implement
{
    public class PartService : IPartService
    {
        /// <summary>
        ///     部位登録
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public bool RegistPart(string partName)
        {
            var model = new PartModel
            {
                PartName = partName,
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
        ///     部位名でソートした部位データを全件取得
        /// </summary>
        /// <returns></returns>
        public List<PartModel> GetPartDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<PartModel>() orderby record.PartName select record;
                return result.Any() ? result.ToList() : new List<PartModel>();
            }
        }

        /// <summary>
        ///     部位更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partName"></param>
        /// <returns></returns>
        public bool UpdatePart(int id, string partName)
        {
            var model = new PartModel
            {
                Id = id,
                PartName = partName
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