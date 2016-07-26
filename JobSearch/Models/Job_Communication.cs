using SQLite.Net.Attributes;
using System;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    //[Table("Job_Communications")]
    public class Job_Communication : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int CommunicationId { get; set; }
        [MaxLength(100)][NotNull]
        public string Via { get; set; }
        [MaxLength(100)][NotNull]
        public string To { get; set; }
        [MaxLength(100)][NotNull]
        public string From { get; set; }
        [MaxLength(100)]
        public string Subject { get; set; }
        [MaxLength(5000)][NotNull]
        public string Description { get; set; }
        [NotNull]
        public DateTime DateAndTime { get; set; }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
