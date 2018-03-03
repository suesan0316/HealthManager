using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("LoadUnit")]
    public class LoadUnitModel
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("LoadId")]
        public int LoadId { get; set; }
        [Column("UnitName")]
        public string UnitName { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
