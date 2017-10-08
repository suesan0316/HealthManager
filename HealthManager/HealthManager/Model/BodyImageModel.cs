using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("BodyImageModel")]
    class BodyImageModel
    {
        [Column("ImageBase64String")]
        public string ImageBase64String { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
