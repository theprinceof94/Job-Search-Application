using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobSearchWebAppWilliams.Models;
using Microsoft.AspNet.Identity;

namespace JobSearchWebAppWilliams.Controllers
{
    public class InterviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Interviews
        public ActionResult Index()
        {
            var interviews = db.Interviews.Include(i => i.JobPosting).Include(i => i.Representative);
            return View(interviews.ToList());
        }

        // GET: Interviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // GET: Interviews/Create
        public ActionResult Create()
        {
            ViewBag.JobPostingID = new SelectList(db.JobPostings, "JobPostingID", "JobPostingID");
            ViewBag.RepresentativeID = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: Interviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InterviewID,JobPostingID,RepresentativeID,Availability,InterviewDate,StartTime,EndTime")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                db.Interviews.Add(interview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobPostingID = new SelectList(db.JobPostings, "JobPostingID", "JobPostingID", interview.JobPostingID);
            ViewBag.RepresentativeID = new SelectList(db.Users, "Id", "FullName", interview.RepresentativeID);
            return View(interview);
        }

        // GET: Interviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobPostingID = new SelectList(db.JobPostings, "JobPostingID", "JobPostingID", interview.JobPostingID);
            ViewBag.RepresentativeID = new SelectList(db.Users, "Id", "FullName", interview.RepresentativeID);
            return View(interview);
        }

        // POST: Interviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InterviewID,JobPostingID,RepresentativeID,Availability,InterviewDate,StartTime,EndTime")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobPostingID = new SelectList(db.JobPostings, "JobPostingID", "JobPostingID", interview.JobPostingID);
            ViewBag.RepresentativeID = new SelectList(db.Users, "Id", "FullName", interview.RepresentativeID);
            return View(interview);
        }

        // GET: Interviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // POST: Interviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interview interview = db.Interviews.Find(id);
            db.Interviews.Remove(interview);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles ="Applicant")]
        [HttpGet]
        public ViewResult SearchInterviews()
        {
            ViewBag.Companies = new SelectList(db.Companies, "CompanyID","CompanyName");

            return View();
        }
        [HttpPost]
        public ViewResult SearchInterviews(int? companyID, DateTime? date, TimeSpan? startTime, TimeSpan? endTime)
        {
            //string applicantID = User.Identity.GetUserId();

            List<Interview> interviewList=Interview.SearchInterviews(companyID,date,startTime,endTime);

            //ViewBag.ApplicantID = applicantID;

            return View("SearchInterviewsResults", interviewList);
        }
    }
}
