﻿using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace JobSearch.Models
{
    //[Table("Recruiters")]
    class Recruiter
    {
        [PrimaryKey, AutoIncrement]
        public int RecruiterId { get; set; }
        [MaxLength(100)][NotNull]
        public string Name { get; set; }
        [MaxLength(100)][Unique][NotNull]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public List<Job> Jobs { get; set; }
    }
}
