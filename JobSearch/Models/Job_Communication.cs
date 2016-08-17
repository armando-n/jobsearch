using SQLite.Net.Attributes;
using System;
using SQLiteNetExtensions.Attributes;

namespace JobSearch.Models
{
    public class Job_Communication : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int CommunicationId { get; set; }

        private string  _via;
        [MaxLength(100)][NotNull]
        public string Via
        {
            get { return _via; }
            set { Set(ref _via, value); }
        }

        private string _to;
        [MaxLength(100)]
        public string To
        {
            get { return _to; }
            set { Set(ref _to, value); }
        }

        private string _from;
        [MaxLength(100)]
        public string From
        {
            get { return _from; }
            set { Set(ref _from, value); }
        }

        private string _subject;
        [MaxLength(100)]
        public string Subject
        {
            get { return _subject; }
            set { Set(ref _subject, value); }
        }

        private string _description;
        [MaxLength(5000)][NotNull]
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        private DateTime? _dateAndTime;
        [NotNull]
        public DateTime? DateAndTime
        {
            get { return _dateAndTime; }
            set { Set(ref _dateAndTime, value); }
        }

        private bool _responseExpected;
        [Default(false, 1)]
        public bool ResponseExpected
        {
            get { return _responseExpected; }
            set { Set(ref _responseExpected, value); }
        }

        [ForeignKey(typeof(Job))][NotNull]
        public int JobId { get; set; }
        [ManyToOne]
        public Job Job { get; set; }
    }
}
