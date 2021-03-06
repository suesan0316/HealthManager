﻿using System;
using SQLite;

namespace HealthManager.Model
{
    [Table("BodyImage")]
    public class BodyImageModel
    {
        [Column("Id"), PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ Column("ImageBase64String") ]
        public string ImageBase64String { get; set; }
        [Column("RegistedDate")]
        public DateTime RegistedDate { get; set; }
    }
}
