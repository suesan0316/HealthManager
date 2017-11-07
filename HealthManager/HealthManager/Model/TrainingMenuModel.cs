using SQLite;

namespace HealthManager.Model
{
    [Table("TrainingMenuModel")]
    class TrainingMenuModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("TrainingId")]
        public int TrainingId { get; set; }
        [Column("Load")]
        public int Load { get; set; }
    }
}
