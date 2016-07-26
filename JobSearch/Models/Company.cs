using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace JobSearch.Models
{
    //[Table("Companies")]
    class Company
    {
        [PrimaryKey, AutoIncrement]
        public int CompanyId { get; set; }
        [MaxLength(100)][Unique]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Domain { get; set; }
        [MaxLength(10)]
        public string Size { get; set; }
        [MaxLength(1000)]
        public string Notes { get; set; }
        [MaxLength(255)]
        public string Website { get; set; }
        [MaxLength(255)]
        public string Linkedin { get; set; }
        [MaxLength(255)]
        public string Glassdoor { get; set; }
        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(2)][NotNull]
        public string State { get; set; }
        //[RegularExpression(@"^\d{5}$")]
        public int? ZipCode { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete | CascadeOperation.CascadeRead)]
        public List<Job> Jobs { get; set; }

        /*public Company()
        {
            if (Jobs == null)
                Jobs = new List<Job>();
        }*/
    }
}
