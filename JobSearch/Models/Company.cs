using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JobSearch.Models
{
    public class Company : Template10.Mvvm.BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int CompanyId { get; set; }
        [MaxLength(100)][Unique][NotNull]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Domain { get; set; }
        [MaxLength(10)]
        public string Size { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        [MaxLength(255)]
        public string Website { get; set; }
        [MaxLength(1000)]
        public string LinkedIn { get; set; }
        [MaxLength(1000)]
        public string Glassdoor { get; set; }
        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(2)][NotNull]
        public string State { get; set; }
        public int? ZipCode { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public ObservableCollection<Job> Jobs { get; set; }
    }
}
