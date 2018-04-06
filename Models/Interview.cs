using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobSearchWebAppWilliams.Models
{
    public class Interview
    {
        [Key]
        public int InterviewID { get; set; }
        [ForeignKey("JobPostingID")]
        public JobPosting JobPosting { get; set; }
        [ForeignKey("RepresentativeID")]
        public Representative Representative { get; set; }
        public int JobPostingID { get; set; }
        [Index("InterviewIndex", IsUnique = true, Order = 1)]
        public string RepresentativeID { get; set; }
        public bool Availability { get; set; }
        [Index("InterviewIndex", IsUnique = true, Order = 2)]
        [DataType(DataType.Date)]
        public DateTime InterviewDate { get; set; }
        [Index("InterviewIndex", IsUnique = true, Order = 3)]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public Interview(int JobPostingID, string repID, DateTime date, TimeSpan start, TimeSpan end)
        {
            this.JobPostingID = JobPostingID;
            this.RepresentativeID = repID;
            this.Availability = true;
            this.InterviewDate = date;
            this.StartTime = start;
            this.EndTime = end;
        }
        public Interview()
        {

        }

        public static List<Interview> PopulateInterview()
        {
            List<Interview> interviewList = new List<Interview>();

            ApplicationDbContext db = new ApplicationDbContext();
            List<Representative> repList = db.Representatives.ToList<Representative>();

            DateTime interviewDate = new DateTime(2017, 12, 25);
            TimeSpan start = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(9, 30, 0);
            TimeSpan increment = new TimeSpan(0, 30, 0);

            Interview interview = new Interview(1, repList[2].RepresentativeID,interviewDate,start,end);

            interviewList.Add(interview);

            start = start.Add(increment);
            end = end.Add(increment);
            interview = new Interview(5, repList[1].RepresentativeID, interviewDate, start, end);

            interviewList.Add(interview);

            start = start.Add(increment);
            end = end.Add(increment);
            interview = new Interview(5, repList[1].RepresentativeID, interviewDate, start, end);
            interview.Availability = false;

            interviewList.Add(interview);

            interviewDate = new DateTime(2017, 12, 10);
            start = start.Add(increment);
            end = end.Add(increment);
            interview = new Interview(5, repList[1].RepresentativeID, interviewDate, start, end);

            interviewList.Add(interview);

            return interviewList;
        }

        public static List<Interview> SearchInterviews(int? companyID, DateTime? date, TimeSpan? startTime, TimeSpan? endTime)
        {
            List<Interview> interviewList = new List<Interview>();

            ApplicationDbContext db = new ApplicationDbContext();

            //db.Representatives.Include("Company");
            DateTime today = DateTime.Today;
            interviewList = db.Interviews.Include("Representative.Company").Include("JobPosting").Where(i=>((i.InterviewDate>today)&&(i.Availability==true))).ToList<Interview>();

            if(companyID !=null)
            {
                interviewList = interviewList.FindAll(i => i.Representative.CompanyID==companyID);
            }

            if(date != null)
            {
                interviewList = interviewList.FindAll(i => i.InterviewDate.Equals(date.Value.Date));
            }

            if (startTime != null)
            {
                interviewList = interviewList.FindAll(i => i.StartTime>=startTime);
            }

            if (endTime != null)
            {
                interviewList = interviewList.FindAll(i => i.EndTime <= endTime);
            }

            return interviewList;
        }
    }
}