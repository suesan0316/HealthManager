using System;
using SQLite;

namespace HealthManager.Model
{
	[Table("Part")]
	public class PartModel
	{
		[Column("Id"), PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Column("PartName")]
		public string PartName { get; set; }
		[Column("RegistedDate")]
		public DateTime RegistedDate { get; set; }
	}
}
