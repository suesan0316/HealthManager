using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HealthManager.Common.Constant;
using Newtonsoft.Json;
using SQLite;

namespace HealthManager.Model.Service
{
    /// <summary>
    /// 体格サービスクラス
    /// </summary>
    public class BodyImageService
    {
        /// <summary>
        /// 体格登録
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static bool RegistBodyImage(string base64String)
        {
            var model = new BodyImageModel
            {
                ImageBase64String = base64String,
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
        /// 体格更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static bool UpdateBodyImage(int id, string base64String)
        {
            var model = new BodyImageModel
            {
                Id = id,
                ImageBase64String = base64String,
                RegistedDate = DateTime.Now
            };
            var s = JsonConvert.SerializeObject(model);
            Debug.WriteLine(s);
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
        /// 体格取得
        /// </summary>
        /// <returns>最新日付の体格</returns>
        public static BodyImageModel GetBodyImage()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<BodyImageModel>()
                    orderby record.RegistedDate descending
                    select record;
                return result.Any() ? result.First() : null;
            }
        }

        /// <summary>
        /// 体格取得
        /// </summary>
        /// <returns>登録日時でソートした体格全件</returns>
        public static List<BodyImageModel> GetBodyImageList()
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<BodyImageModel>() orderby record.RegistedDate select record;
                return result.Any() ? result.ToList() : new List<BodyImageModel>();
            }
        }

        /// <summary>
        /// 指定日の体格存在チェック
        /// </summary>
        /// <param name="targeTime"></param>
        /// <returns></returns>
        public static bool CheckExitTargetDayData(DateTime targeTime)
        {
            using (var db = new SQLiteConnection(DbConst.DbPath))
            {
                var result = from record in db.Table<BodyImageModel>() select  record;
                return result.Any() && result.ToList().Any(data => data.RegistedDate.Date == targeTime.Date);
            }
        }
    }
}