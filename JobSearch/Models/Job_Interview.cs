using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;

namespace JobSearch.Models
{
    public class Job_Interview : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int InterviewId { get; set; }
        [NotNull]
        public string Via { get; set; }
        [NotNull]
        public DateTime DateAndTime { get; set; }
        [MaxLength(100)]
        public string Interviewer { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
