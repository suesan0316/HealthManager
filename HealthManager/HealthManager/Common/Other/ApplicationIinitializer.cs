using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HealthManager.Common.Constant;
using HealthManager.Common.Extention;
using HealthManager.DependencyInterface;
using HealthManager.Model;
using Newtonsoft.Json;
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
                    db.DropTable<InitModel>();
                    //db.DropTable<InitModel>();
                    db.DropTable<BodyImageModel>();
                    //db.DropTable<BasicDataModel>();
                    db.DropTable<LoadModel>();
                    db.DropTable<PartModel>();
                    db.DropTable<SubPartModel>();
                    db.DropTable<LoadUnitModel>();
                    db.DropTable<LocationModel>();
                   db.DropTable<TrainingMasterModel>();
                    db.DropTable<TrainingMenuModel>();
                    db.DropTable<TrainingScheduleModel>();
                    //db.DropTable<TrainingResultModel>();

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
                        db.CreateTable<LoadUnitModel>();
                        db.CreateTable<LocationModel>();
                        db.CreateTable<TrainingResultModel>();
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
            ReadingLoadMaster();
            // 負荷単位マスタを読み込み
            ReadingLoadUnitMaster();
            // ロケーションマスタを読み込み
            ReadingLocationMaster();

            //TODO 消す
            //TestDataReadOfBasicData();
            //TestDataReadOfBodyData();
            //TestDataReadOfTrainingData();
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
            // 負荷マスタを読み込む
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

        public static void ReadingLoadUnitMaster()
        {
            // 負荷マスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.load_unit_data.csv");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        db.Insert(new LoadUnitModel()
                        {
                            Id = int.Parse(row.Split(CharConst.Conma)[0]),
                            LoadId = int.Parse(row.Split(CharConst.Conma)[1]),
                            UnitName = row.Split(CharConst.Conma)[2],
                            RegistedDate = new DateTime()
                        });
                    }
                }
            }
        }

        public static void ReadingLocationMaster()
        {
            // ロケーションマスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.location_data.csv");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        db.Insert(new LocationModel()
                        {
                            Id = int.Parse(row.Split(CharConst.Conma)[0]),
                            LocationName = row.Split(CharConst.Conma)[1],
                            CityId = row.Split(CharConst.Conma)[2],
                            RegistedDate = new DateTime()
                        });
                    }
                }
            }
        }

        public static void TestDataReadOfBasicData()
        {
            // ロケーションマスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.Test.basic_data_test.txt");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        var models = JsonConvert.DeserializeObject<List<BasicDataModel>>(row);
                        models.ForEach(data => db.Insert(data));
                    }
                }
            }
        }

        public static void TestDataReadOfBodyData()
        {
            // ロケーションマスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.Test.body_data_test.txt");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        var models = JsonConvert.DeserializeObject<BodyImageModel>(row);
                        db.Insert(models);
                    }
                }
            }
        }

        public static void TestDataReadOfTrainingData()
        {
            // ロケーションマスタを読み込む
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(typeof(ApplicationIinitializer).Namespace + ".Data.Test.training_data_test.txt");
            using (var reader = new System.IO.StreamReader(stream))
            {
                using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
                {
                    while (!reader.EndOfStream)
                    {
                        var row = reader.ReadLine();
                        var models = JsonConvert.DeserializeObject<TrainingMasterModel>(row);
                        db.Insert(models);
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
