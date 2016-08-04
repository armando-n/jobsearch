﻿using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSearch.Models;
using System.Collections.ObjectModel;

namespace JobSearch.Services.DatabaseService
{
    public class DatabaseService : Template10.Mvvm.BindableBase
    {
        private static DatabaseService instance;
        private SQLite.Net.SQLiteConnection connection;

        List<Company> _companies;
        public List<Company> Companies
        {
            get
            {
                //if (_companies == null)
                //{
                //    _companies = getConnection().GetAllWithChildren<Company>();
                //}

                return _companies;
            }
            set
            {
                _companies = value;
            }
        }

        List<Recruiter> _recruiters;
        public List<Recruiter> Recruiters
        {
            get
            {
                //if (_recruiters == null)
                //{
                //    _recruiters = getConnection().GetAllWithChildren<Recruiter>();
                //}

                return _recruiters;
            }
            set
            {
                _recruiters = value;
            }
        }
        List<Job> _jobs;
        public List<Job> Jobs
        {
            get
            {
                return _jobs;// ?? getConnection().GetAllWithChildren<Job>();

                //if (_jobs == null)
                //{
                //    _jobs = getConnection().GetAllWithChildren<Job>();
                //}

                //return _jobs;
            }
            set
            {
                _jobs = value;
            }
        }
        public List<Job_Communication> Communications { get; set; }
        public List<Job_Interview> Interviews { get; set; }
        List<Job_Requirement> _requirements;
        public List<Job_Requirement> Requirements
        {
            get { return _requirements; }
            set { _requirements = value; }
        }
        public List<Job_Responsibility> Responsibilities { get; set; }
        public List<Job_Test> Tests { get; set; }

        private DatabaseService()
        {
            //var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "JobSearch.sqlite");
            //db = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            //getConnection().TraceListener = new DebugTraceListener();
            //getConnection();
            //InitializeTables();
            //PopulateDatabase();
            _jobs = getConnection().GetAllWithChildren<Job>();
            _companies = getConnection().GetAllWithChildren<Company>();
            _recruiters = getConnection().GetAllWithChildren<Recruiter>();
            _requirements = getConnection().GetAllWithChildren<Job_Requirement>();
            //this.Close();
        }

        public static DatabaseService GetDB()
        {
            if (instance == null)
                instance = new DatabaseService();

            return instance;
        }

        //public List<Job> GetJobs()
        //{
        //    return Jobs;
        //}

        public void Close()
        {
            getConnection().Close();
        }

        public void AddJob(Job job, string companyName, string recruiterName)
        {
            Company company;
            Recruiter recruiter;

            getConnection().Insert(job);
            Jobs.Add(job);

            company = Companies.Where(cmpny => string.Equals(cmpny.Name, companyName)).Single();
            company.Jobs.Add(job);
            getConnection().UpdateWithChildren(company);

            if (! String.IsNullOrWhiteSpace(recruiterName))
            {
                IEnumerable<Recruiter> candidateRecruiters = Recruiters.Where(rcrtr => string.Equals(rcrtr.Name, recruiterName));
                recruiter = candidateRecruiters.Single();
                recruiter.Jobs.Add(job);
                getConnection().UpdateWithChildren(recruiter);
            }
        }

        public void AddCompany(Company company)
        {
            getConnection().Insert(company);
            Companies.Add(company);
        }

        public void AddRecruiter(Recruiter recruiter)
        {
            getConnection().Insert(recruiter);
            Recruiters.Add(recruiter);
        }

        public void AddJobRequirement(Job_Requirement jobRequirement, int jobId)
        {
            Job job;

            getConnection().Insert(jobRequirement);
            Requirements.Add(jobRequirement);

            job = Jobs.Where(aJob => aJob.JobId == jobId).Single();
            job.Requirements.Add(jobRequirement);
            getConnection().UpdateWithChildren(job);
        }

        private SQLite.Net.SQLiteConnection getConnection()
        {
            if (connection == null)
            {
                var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "JobSearch.sqlite");
                connection = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
                connection.TraceListener = new DebugTraceListener();
            }
            return connection;
        }

        private void InitializeTables()
        {
            getConnection().CreateTable<Company>();
            getConnection().CreateTable<Recruiter>();
            getConnection().CreateTable<Job>();
            getConnection().CreateTable<Job_Communication>();
            getConnection().CreateTable<Job_Interview>();
            getConnection().CreateTable<Job_Requirement>();
            getConnection().CreateTable<Job_Responsibility>();
            getConnection().CreateTable<Job_Test>();
        }

        private void PopulateDatabase()
        {
            Company weston = new Company()
            {
                Name = "The Weston Group",
                Domain = "healthcare industry",
                Size = "medium",
                Notes = "use a wide range of technologies",
                Website = "http://weston.com",
                Glassdoor = "https://www.glassdoor.com/Overview/Working-at-The-Weston-Group-EI_IE678512.11,27.htm",
                StreetAddress = "10101 Southwest Freeway, Suit 500",
                City = "Houston",
                State = "TX",
                ZipCode = 77074,
                Jobs = new List<Job>()
            };
            Company ghgCorporation = new Company()
            {
                Name = "GHG Corporation",
                Domain = "general software",
                Size = "large",
                Notes = "use a wide rang of technologies",
                Website = "http://www.ghgcorp.com/",
                Linkedin = "https://www.linkedin.com/company/ghg-corporation?trk=top_nav_home",
                Glassdoor = "https://www.glassdoor.com/Overview/Working-at-GHG-Corp-EI_IE139275.11,19.htm",
                StreetAddress = "960 Clear Lake City Blvd.",
                City = "Webster",
                State = "TX",
                ZipCode = 77598,
                Jobs = new List<Job>()
            };
            Company cyberCoders = new Company()
            {
                Name = "CyberCoders",
                Domain = "employment, staffing, and recruiting",
                Size = "large",
                Website = "https://www.cybercoders.com/",
                Linkedin = "https://www.linkedin.com/company/cybercoders?trk=top_nav_home",
                Glassdoor = "https://www.glassdoor.com/Overview/Working-at-GHG-Corp-EI_IE139275.11,19.htm",
                StreetAddress = "6591 Irvine Center Drive Suite 200",
                City = "Irving",
                State = "CA",
                ZipCode = 92618,
                Jobs = new List<Job>()
            };
            Company quadriviumSystems = new Company()
            {
                Name = "Quadrivium Systems",
                Domain = "general software",
                Website = "http://www.quadriviumsystems.com/",
                City = "Houston",
                State = "TX",
                Jobs = new List<Job>()
            };
            Company poeticSystems = new Company()
            {
                Name = "Poetic Systems",
                Domain = "general software",
                Size = "medium",
                Notes = "have nice website portfolio",
                Website = "http://poeticsystems.com/",
                Linkedin = "https://www.linkedin.com/company/poetic-systems-llc?trk=top_nav_home",
                Glassdoor = "https://www.glassdoor.com/Overview/Working-at-Poetic-Systems-EI_IE868736.11,25.htm",
                StreetAddress = "675 Bering Dr. Ste. 725",
                City = "Houston",
                State = "TX",
                ZipCode = 77057,
                Jobs = new List<Job>()
            };

            getConnection().Insert(weston);
            getConnection().Insert(ghgCorporation);
            getConnection().Insert(cyberCoders);
            getConnection().Insert(quadriviumSystems);
            getConnection().Insert(poeticSystems);

            Recruiter bryanKuna = new Recruiter()
            {
                Name = "Bryan Kuna",
                Email = "Bryan.Kuna@CyberCoders.com"
            };
            Recruiter iraDSilva = new Recruiter()
            {
                Name = "Ira D'Silva",
                Email = "ira@revature.com"
            };

            getConnection().Insert(bryanKuna);
            getConnection().Insert(iraDSilva);

            Job westonSoftwareDeveloper_001 = new Job()
            {
                Position = "Software Developer",
                DatePosted = new DateTime(2016, 7, 6),
                DateApplied = new DateTime(2016, 7, 11, 21, 34, 0),
                EmploymentService = "monster.com",
                AppliedViaWebsite = true,
                AppliedViaEmail = true,
                StreetAddress = "10101 Southwest Freeway, Suite 500",
                City = "Houston",
                State = "TX",
                ZipCode = 77074,
                Notes = "still waiting to hear about IQ test results",
                Communications = new ObservableCollection<Job_Communication>(),
                Interviews = new ObservableCollection<Job_Interview>(),
                Requirements = new ObservableCollection<Job_Requirement>(),
                Responsibilities = new ObservableCollection<Job_Responsibility>(),
                Tests = new ObservableCollection<Job_Test>()
            };
            getConnection().Insert(westonSoftwareDeveloper_001);
            weston.Jobs.Add(westonSoftwareDeveloper_001);
            getConnection().UpdateWithChildren(weston);

            Job_Test westonIQTest = new Job_Test()
            {
                DateAndTime = new DateTime(2016, 07, 18, 13, 30, 0),
                Type = "online",
                Notes = "45 min IQ test. I scored 115."
            };
            getConnection().Insert(westonIQTest);
            westonSoftwareDeveloper_001.Tests.Add(westonIQTest);
            getConnection().UpdateWithChildren(westonSoftwareDeveloper_001);

            var storedCompany = getConnection().GetWithChildren<Company>(weston.CompanyId, recursive: true);
            if (weston.Jobs.First<Job>().Position.Equals(storedCompany.Jobs.First<Job>().Position))
            {
                Debug.WriteLine("Object and relationships loaded correctly!");
            }
        }
    }
}
