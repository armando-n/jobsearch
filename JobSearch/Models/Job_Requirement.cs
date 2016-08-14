using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    public class Job_Requirement : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int RequirementId { get; set; }

        private string _requirement;
        [MaxLength(511)][NotNull]
        public string Requirement
        {
            get { return _requirement; }
            set { Set(ref _requirement, value); }
        }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
