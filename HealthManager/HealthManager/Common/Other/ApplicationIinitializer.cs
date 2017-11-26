using System.Linq;
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
        public void InitDatabase()
        {
            using (var db = new SQLiteConnection(DependencyService.Get<ISqliteDeviceInform>().GetDbPath()))
            {
                try
                {
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
                
                if (!(from record in db.Table<InitModel>() select record).Any())
                {
                    db.CreateTable<InitModel>();
                    db.Insert(new InitModel());
                    try
                    {
                        db.CreateTable<BodyImageModel>();
                        db.CreateTable<BasicDataModel>();
                        db.CreateTable<LoadModle>();
                        db.CreateTable<PartModel>();
                        db.CreateTable<SubPartModel>();
                        db.CreateTable<TrainingMasterModel>();
                        db.CreateTable<TrainingMenuModel>();
                        db.CreateTable<TrainingScheduleModel>();
                    }
                    catch (NotNullConstraintViolationException e)
                    {

                    }
                }
            }               
        }
        public static void Init()
        {
            
        }
    }

    [Table("Init")]
    public class InitModel
    {
        [Column("Id"),PrimaryKey,AutoIncrement]
        public int Id { get; set; }
    }
}
