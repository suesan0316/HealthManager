using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("Load")]
    public class LoadModel
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("LoadName")]
        public string LoadName { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
