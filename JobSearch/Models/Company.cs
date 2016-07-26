using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Models
{
    [Table("Companies")]
    class Company
    {
        public int CompanyId { get; set; }
        [MaxLength(100)]
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
        [MaxLength(2)]
        public string State { get; set; }
        [RegularExpression(@"^\d{5}$")]
        public int? ZipCode { get; set; }

        public List<Job> Jobs { get; set; }

        /*public Company()
        {
            if (Jobs == null)
                Jobs = new List<Job>();
        }*/
    }
}
