using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;

namespace JobSearch.Models
{
    public class Job_Test : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int TestId { get; set; }
        [NotNull]
        public string Type { get; set; }
        [NotNull]
        public DateTime? DateAndTime { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
