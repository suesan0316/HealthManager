using System;
using SQLite;

namespace HealthManager.Model
{
	[Table("BodyImageModel")]
	public class TrainingMasterModel
    {
	    [PrimaryKey, AutoIncrement]
	    public int Id { get; set; }
	    [Column("TrainingName")]
	    public string TrainingName { get; set; }
	    [Column("LoadType")]
	    public int LoadType { get; set; }
		[Column("PartId")]
		public int PartId { get; set; }
		[Column("SubPartId")]
		public int SubPartId { get; set; }
	    [Column("RegistedDate")]
	    public DateTime RegistedDate { get; set; }
	}
}
