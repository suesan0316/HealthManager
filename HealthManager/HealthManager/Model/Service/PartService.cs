﻿using System;
using System.Collections.Generic;
using System.Linq;
using HealthManager.Common.Constant;
using SQLite;

namespace HealthManager.Model.Service
{
    /// <summary>
    /// 部位サービスクラス
    /// </summary>
    public class PartService
    {
        /// <summary>
        /// 部位登録
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public static bool RegistPart(string partName)
        {
            var model = new PartModel
            {
                PartName = partName,
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
        /// 部位名でソートした部位データを全件取得
        /// </summary>
        /// <returns></returns>
        public static List<PartModel> GetPartDataList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<PartModel>() orderby record.PartName select record;
                return result.Any() ? result.ToList() : new List<PartModel>();
            }
        }

        /// <summary>
        /// 部位更新
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="partName"></param>
        /// <returns></returns>
        public static bool UpdatePart(int Id ,string partName)
        {
            var model = new PartModel
            {
                Id = Id,
                PartName = partName
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
