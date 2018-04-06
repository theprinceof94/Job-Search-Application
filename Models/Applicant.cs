using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobSearchWebAppWilliams.Models
{
    public class Applicant : ApplicationUser
    {
        //db stuff
        [Key]
        public string ApplicantID { get; set; }//only for classes inheriting from ApplicationUser, the ID is  string instead of int
        [ForeignKey("ApplicantID")]
        public ApplicationUser ApplicationUser { get; set; }

        public Applicant(string fullname, string email, string phonenumber,string applicantType):base(fullname,email,phonenumber)
        {
            this.ApplicantType = applicantType;
            this.ApplicantResume = null;
        }
        public Applicant()
        {

        }

        public static List<Applicant> PopulateApplicants()
        {
            List<Applicant> applicantList = new List<Applicant>();

            Applicant applicant = new Applicant("TestStudent$1","teststudent@test.com","(xxx)xxx-xxxx", "Student");
            applicant.ApplicantID = applicant.Id;
            applicantList.Add(applicant);

            applicant = new Applicant("TestAlum$1", "tesalumt@test.com", "(xxx)xxx-xxxx", "Alum");
            applicant.ApplicantID = applicant.Id;
            applicantList.Add(applicant);


            return applicantList;
        }
        //properties
        public byte[] ApplicantResume { get; set; }
        public string ApplicantType { get; set; }
    }
}