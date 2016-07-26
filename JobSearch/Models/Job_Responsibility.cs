using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    //[Table("Job_Responsibilities")]
    class Job_Responsibility
    {
        [PrimaryKey, AutoIncrement]
        public int ResponsibilityId { get; set; }
        [MaxLength(255)][NotNull]
        public string Responsibility { get; set; }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
