using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("BasicDataModel")]
    public class BasicDataModel
    {
        public string Name { get; set; }
        public bool Sex { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double BodyWeight { get; set; }
        public int MaxBloodPressure { get; set; }
        public int MinBloodPressure { get; set; }
        public DateTime RegistedDate { get; set; }
    }
}
 