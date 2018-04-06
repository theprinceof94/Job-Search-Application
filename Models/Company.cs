using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JobSearchWebAppWilliams.Models
{
    public class Company
    {
        //public string CompanyHeadquarters { get; set; }
        public string CompanyName { get; set; }
        [Key]
        public int CompanyID { get; set; }
        public int CompanySize { get; set; }

        public Company(string companyname, int companysize)
        {
            this.CompanyName = companyname;
            this.CompanySize = companysize;
        }
        public Company()
        {

        }

        public static List<Company> PopulateCompanies()
        {
            List<Company> companyList = new List<Company>();

            Company company = new Company("Deloitte",225351);
            companyList.Add(company);

            company = new Company("Plus", 200);
            companyList.Add(company);

            return companyList;
        }

        ApplicationDbContext db = new ApplicationDbContext();
        public bool? AddCompany(out string exceptionMessage)
        {
            bool? success = null;
            
            try
            {
                db.Companies.Add(this);
                db.SaveChanges();
                success = true;
                exceptionMessage = "None";
            }
            catch(Exception exception)
            {
                success = false;
                exceptionMessage = exception.Message;
            }

            return success;
        }

        public bool? DeleteCompany(out string exceptionMessage)
        {
            bool? success = null;
            int companyID = this.CompanyID;
            try
            {
                Company company = db.Companies.Find(companyID);
                db.Companies.Remove(this);
                db.SaveChanges();
                success = true;
                exceptionMessage = "None";
            }
            catch (Exception exception)
            {
                success = false;
                exceptionMessage = exception.Message;
            }

            return success;
        }

        public bool? EditCompany()
        {
            bool? editSucceeded = null;
            try
            {
                db.Entry(this).State = EntityState.Modified;
                db.SaveChanges();
                editSucceeded = true;
            }
            catch(Exception e)
            {
                editSucceeded = false;
            }
            return editSucceeded;
        }
    }
}