using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    public class Job_Responsibility : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int ResponsibilityId { get; set; }

        private string _responsibility;
        [MaxLength(511)][NotNull]
        public string Responsibility
        {
            get { return _responsibility; }
            set { Set(ref _responsibility, value); }
        }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
