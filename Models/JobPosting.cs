using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobSearchWebAppWilliams.Models
{
    public class JobPosting
    {
        [Key]
        public int JobPostingID { get; set; }
        public JobPosting()
        {

        }

        public static List<JobPosting> PopulateJobPosting()
        {
            List<JobPosting> jobList = new List<JobPosting>();
            JobPosting job = new JobPosting();
            jobList.Add(job);
            job = new JobPosting();
            jobList.Add(job);
            job = new JobPosting();
            jobList.Add(job);
            job = new JobPosting();
            jobList.Add(job);
            job = new JobPosting();
            jobList.Add(job);
            job = new JobPosting();
            jobList.Add(job);
            job = new JobPosting();
            jobList.Add(job);

            return jobList;
        }
    }
}