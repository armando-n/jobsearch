using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    public class Job_Responsibility : Template10.Mvvm.BindableBase
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
