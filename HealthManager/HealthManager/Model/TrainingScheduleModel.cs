using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("TrainingSchedule")]
    public class TrainingScheduleModel
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("TrainingMenuId")]
        public int TrainingMenuId { get; set; }
        [Column("Week")]
        public int Week { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
