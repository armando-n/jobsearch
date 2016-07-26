using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    //[Table("Job_Requirements")]
    class Job_Requirement
    {
        [PrimaryKey, AutoIncrement]
        public int RequirementId { get; set; }
        [MaxLength(255)][NotNull]
        public string Requirement { get; set; }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
