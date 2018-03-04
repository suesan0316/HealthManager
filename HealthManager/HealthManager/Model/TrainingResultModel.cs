using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("TrainingResult")]
    public class TrainingResultModel
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("TrainingContent")]
        public string TrainingContent { get; set; }
        [Column("Weather")]
        public string Weather { get; set; }
        [Column("TargetDate")]
        public DateTime TargetDate { get; set; }
        [Column("StartDate")]
        public DateTime StartDate { get; set; }
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
