using System;
using System.Linq;
using System.Reflection;
using HealthManager.Common.Constant;
using HealthManager.DependencyInterface;
using HealthManager.Model;
using SQLite;
using Xamarin.Forms;

namespace HealthManager.Common.Other
{
    /// <summary>
    /// アプリケーション初期化クラス
    /// </summary>
    public class ApplicationIinitializer
    {
        public static void InitDatabase()
        {
            using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
            {
                try
                {
                    //db.DropTable<InitModel>();
                    //db.DropTable<BodyImageModel>();
                    //db.DropTable<BasicDataModel>();
                    //db.DropTable<LoadModle>();
                    //db.DropTable<PartModel>();
                    //db.DropTable<SubPartModel>();
                    //db.DropTable<TrainingMasterModel>();
                    //db.DropTable<TrainingMenuModel>();
                    //db.DropTable<TrainingScheduleModel>();
                }
                catch (NotNullConstraintViolationException e)
                {

                }

                db.CreateTable<InitModel>();
                if (!(from record in db.Table<InitModel>() select record).Any())
                {
                    db.Insert(new InitModel());
                    try
                    {
                        db.CreateTable<BodyImageModel>();
                        db.CreateTable<BasicDataModel>();
                        db.CreateTable<LoadModel>();
                        db.CreateTable<PartModel>();
                        db.CreateTable<SubPartModel>();
                        db.CreateTable<TrainingMasterModel>();
                        db.CreateTable<TrainingMenuModel>();
                        db.CreateTable<TrainingScheduleModel>();
                    }
                    catch (NotNullConstraintViolationException e)
                    {
                        throw e;
                    }
                    InsertMasterData();
                }
            }               
        }

        public static void InsertMasterData()
        {
            // 部位マスタを読み込む
            ReadingPartMaster();
            // サブ部位マスタを読み込み
            ReadingSubPartMaster();
            // 負荷マスタを読み込み
        }

        public static void ReadingPartMaster()
        {
            // 部位マスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.part_data.csv");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        db.Insert(new PartModel()
                        {
                            Id = int.Parse(row.Split(CharConst.Conma)[0]),
                            PartName = row.Split(CharConst.Conma)[1],
                            RegistedDate = new DateTime()
                        });
                    }
                }
            }
        }

        public static void ReadingSubPartMaster()
        {
            // サブ部位マスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.sub_part_data.csv");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        db.Insert(new SubPartModel()
                        {
                            ParentPartId = int.Parse(row.Split(CharConst.Conma)[0]),
                            SubPartName = row.Split(CharConst.Conma)[1],
                            RegistedDate = new DateTime()
                        });
                    }
                }
            }
        }

        public static void ReadingLoadMaster()
        {
            // 部位マスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.load_data.csv");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        db.Insert(new LoadModel()
                        {
                            Id = int.Parse(row.Split(CharConst.Conma)[0]),
                            LoadName = row.Split(CharConst.Conma)[1],
                            RegistedDate = new DateTime()
                        });
                    }
                }
            }
        }

        public static void Init()
        {
            
        }
    }

    [Table("Init")]
    internal class InitModel
    {
        [Column("Id"),PrimaryKey,AutoIncrement]
        public int Id { get; set; }
    }
}
