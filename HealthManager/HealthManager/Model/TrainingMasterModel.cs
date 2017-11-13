using System;
using SQLite;

namespace HealthManager.Model
{
	[Table("TrainingMaster")]
	public class TrainingMasterModel
    {
	    [Column("Id"), PrimaryKey, AutoIncrement]
	    public int Id { get; set; }
	    [Column("TrainingName")]
	    public string TrainingName { get; set; }
	    [Column("Load")]
	    public string Load { get; set; }
		[Column("Part")]
		public string Part { get; set; }
	    [Column("RegistedDate")]
	    public DateTime RegistedDate { get; set; }
	}
}
