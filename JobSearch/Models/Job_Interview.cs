using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;

namespace JobSearch.Models
{
    public class Job_Interview : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int InterviewId { get; set; }

        private string _via;
        [NotNull]
        public string Via
        {
            get { return _via; }
            set { Set(ref _via, value); }
        }

        private DateTime? _dateAndTime;
        [NotNull]
        public DateTime? DateAndTime
        {
            get { return (_dateAndTime?.Kind == DateTimeKind.Utc) ? _dateAndTime?.ToLocalTime() : _dateAndTime; }
            set { Set(ref _dateAndTime, value); }
        }

        private string _interviewer;
        [MaxLength(100)]
        public string Interviewer
        {
            get { return _interviewer; }
            set { Set(ref _interviewer, value); }
        }

        private string _notes;
        [MaxLength(1000)]
        public string Notes
        {
            get { return _notes; }
            set { Set(ref _notes, value); }
        }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
