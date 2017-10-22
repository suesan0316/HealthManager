using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("BasicDataModel")]
    public class BasicDataModel
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public float Height { get; set; }
        public float BodyWeight { get; set; }
        public float BodyFatPercentage { get; set; }
        public int MaxBloodPressure { get; set; }
        public int MinBloodPressure { get; set; }
        public int BasalMetabolism { get; set; }
        public DateTime RegistedDate { get; set; }
    }
}
 