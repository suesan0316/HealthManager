using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("BodyImageModel")]
    public class BodyImageModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ Column("ImageBase64String") ]
        public string ImageBase64String { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
