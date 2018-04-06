using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobSearchWebAppWilliams.Models
{
    public class ApplicantInterview
    {
        [Key]
        public int ApplicantInterviewID { get; set; }
        public bool? ApplicantRequest { get; set; }
        [Index("ApplicationInterviewIndex", IsUnique = true, Order = 1)]
        public int? InterviewID { get; set; }
        [ForeignKey("InterviewID")]
        public Interview Interview { get; set; }
        [Index("ApplicationInterviewIndex", IsUnique = true, Order = 2)]
        public string ApplicantID { get; set; }
        [ForeignKey("ApplicantID")]
        public Applicant Applicant { get; set; }

        public ApplicantInterview()
        {

        }
        public ApplicantInterview(string applicantID,int? interviewID)
        {
            this.ApplicantID = applicantID;
            this.InterviewID = interviewID;
            this.ApplicantRequest = null;
        }


    }
}