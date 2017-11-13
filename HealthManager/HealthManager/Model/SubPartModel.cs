using System;
using SQLite;

namespace HealthManager.Model
{
	[Table("SubPart")]
	public class SubPartModel
	{
		[Column("Id"), PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Column("ParentPartId")]
		public  int ParentPartId { get; set; }
		[Column("SubPartName")]
		public string SubPartName { get; set; }
		[Column("RegistedDate")]
		public DateTime RegistedDate { get; set; }
	}
}
