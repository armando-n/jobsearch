using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobSearch.Models
{
    [Table("Recruiters")]
    class Recruiter
    {
        public int RecruiterId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        public List<Job> Jobs { get; set; }
    }
}
