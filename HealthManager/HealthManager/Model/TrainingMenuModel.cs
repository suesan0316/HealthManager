using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("TrainingMenu")]
    public class TrainingMenuModel
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("MenuName")]
        public string MenuName { get; set; }
        [Column("TrainingId")]
        public int TrainingId { get; set; }
        [Column("Load")]
        public string Load { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
