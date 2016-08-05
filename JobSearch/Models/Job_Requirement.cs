using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    public class Job_Requirement : Template10.Mvvm.BindableBase
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
