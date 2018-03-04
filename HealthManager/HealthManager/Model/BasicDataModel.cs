using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("BasicData")]
    public class BasicDataModel
    {
        [Column("Id"), PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("LocationId")]
        public int LocationId { get; set; }
        [Column("Gender")]
        public int Gender { get; set; }
        [Column("Age")]
        public int Age { get; set; }
        [Column("Height")]
        public float Height { get; set; }
        [Column("BodyWeight")]
        public float BodyWeight { get; set; }
        [Column("BodyFatPercentage")]
        public float BodyFatPercentage { get; set; }
        [Column("MaxBloodPressure")]
        public int MaxBloodPressure { get; set; }
        [Column("MinBloodPressure")]
        public int MinBloodPressure { get; set; }
        [Column("BasalMetabolism")]
        public int BasalMetabolism { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
 