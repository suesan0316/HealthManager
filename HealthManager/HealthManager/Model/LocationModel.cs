using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("Locationnw")]
    public class LocationModel
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("LocationName")]
        public string LocationName { get; set; }
        [Column("CityId")]
        public string CityId { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
