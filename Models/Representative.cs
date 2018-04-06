using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobSearchWebAppWilliams.Models
{
    public class Representative : ApplicationUser
    {
        //db stuff
        [Key]
        public string RepresentativeID { get; set; }//only for classes inheriting from ApplicationUser, the ID is  string instead of int
        [ForeignKey("RepresentativeID")]
        public ApplicationUser ApplicationUser { get; set; }

        public int CompanyID { get; set; }
        [ForeignKey("CompanyID")]
        public Company Company { get; set; }
        //properties

        public Representative(string fullname, string email, string phonenumber, int companyid):base(fullname,email,phonenumber)
        {
            this.CompanyID = companyid;
        }
        public Representative()
        {

        }

        public static List<Representative> PopulateRepresentatives()
        {
            List<Representative> repList = new List<Representative>();

            Representative rep = new Representative("TestRepDeloitte1", "testd1@test.com", "(xxx)xxx-xxxx",1);
            rep.RepresentativeID = rep.Id;
            repList.Add(rep);
            rep = new Representative("TestRepDeloitte2", "testd2@test.com", "(xxx)xxx-xxxx",1);
            rep.RepresentativeID = rep.Id;
            repList.Add(rep);
            rep = new Representative("TestRepPlus1", "testp1@test.com", "(xxx)xxx-xxxx",2);
            rep.RepresentativeID = rep.Id;
            repList.Add(rep);
            rep = new Representative("TestRepPlus2", "testp2@test.com", "(xxx)xxx-xxxx",2);
            rep.RepresentativeID = rep.Id;
            repList.Add(rep);

            return repList;
        }
    }
}