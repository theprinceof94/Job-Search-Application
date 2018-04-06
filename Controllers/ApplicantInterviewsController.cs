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
    public class ApplicantInterviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]

        public ActionResult EditSave(int ApplicantInterviewID, string ChangeRequestStatus)
        {

            ApplicantInterview applicantInterview = db.ApplicantInterviews.Find(ApplicantInterviewID);

            bool? requestStatus = null;
            if (ChangeRequestStatus == "True")
            {
                requestStatus = true;
            }
            else if (ChangeRequestStatus == "False")
            {
                requestStatus = false;
            }

            if (applicantInterview.ApplicantRequest != requestStatus)
            {

                applicantInterview.ApplicantRequest = requestStatus;
                db.Entry(applicantInterview).State = EntityState.Modified;
                db.SaveChanges();

                Interview interview = db.Interviews.Find(applicantInterview.InterviewID);
                if (requestStatus == true)
                {
                    interview.Availability = false;
                    db.Entry(interview).State = EntityState.Modified;
                    db.SaveChanges();
                }

                if(requestStatus==false||requestStatus==null)
                {
                    interview.Availability = true;
                    db.Entry(interview).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ListRequestedInterviews");
        }
           
        [Authorize(Roles ="Applicant")]
        public ActionResult RequestInterview(int? id)
        {
            string applicantID = User.Identity.GetUserId();

            ApplicantInterview applicantInterview = new ApplicantInterview(applicantID, id);

            try
            {
                db.ApplicantInterviews.Add(applicantInterview);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Representative")]
        public ActionResult ListRequestedInterviews()
        {
            var selectList = new List<SelectListItem>();
            SelectListItem selectListItem = new SelectListItem();
            selectListItem.Text = "Not Set";
            selectListItem.Value ="Not Set";
            selectList.Add(selectListItem);

            selectListItem = new SelectListItem();
            selectListItem.Text = "True";
            selectListItem.Value = "True";
            selectList.Add(selectListItem);

            selectListItem = new SelectListItem();
            selectListItem.Text = "False";
            selectListItem.Value = "False";
            selectList.Add(selectListItem);

            ViewBag.StatusOptions=selectList;

            string repID = User.Identity.GetUserId();

            var requestedInterviews = db.ApplicantInterviews.Include(a => a.Applicant).Include(a => a.Interview).Include(a => a.Interview.Representative).Where(a => a.Interview.RepresentativeID == repID);
            return View(requestedInterviews.ToList());

        }
        // GET: ApplicantInterviews
        public ActionResult Index()
        {
            string applicantID = User.Identity.GetUserId();

            var applicantInterviews = db.ApplicantInterviews.Include(a => a.Applicant).Include(a => a.Interview.Representative.Company).Where(a => a.ApplicantID==applicantID);
            return View(applicantInterviews.ToList());
        }

        // GET: ApplicantInterviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantInterview applicantInterview = db.ApplicantInterviews.Find(id);
            if (applicantInterview == null)
            {
                return HttpNotFound();
            }
            return View(applicantInterview);
        }

        // GET: ApplicantInterviews/Create
        public ActionResult Create()
        {
            ViewBag.ApplicantID = new SelectList(db.Users, "Id", "FullName");
            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "RepresentativeID");
            return View();
        }

        // POST: ApplicantInterviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicantInterviewID,ApplicantRequest,InterviewID,ApplicantID")] ApplicantInterview applicantInterview)
        {
            if (ModelState.IsValid)
            {
                db.ApplicantInterviews.Add(applicantInterview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicantID = new SelectList(db.Users, "Id", "FullName", applicantInterview.ApplicantID);
            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "RepresentativeID", applicantInterview.InterviewID);
            return View(applicantInterview);
        }

        // GET: ApplicantInterviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantInterview applicantInterview = db.ApplicantInterviews.Find(id);
            if (applicantInterview == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantID = new SelectList(db.Users, "Id", "FullName", applicantInterview.ApplicantID);
            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "RepresentativeID", applicantInterview.InterviewID);
            return View(applicantInterview);
        }

        // POST: ApplicantInterviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicantInterviewID,ApplicantRequest,InterviewID,ApplicantID")] ApplicantInterview applicantInterview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicantInterview).State = EntityState.Modified;
                db.SaveChanges();

                Interview interview =db.Interviews.Find(applicantInterview.InterviewID);
                if (applicantInterview.ApplicantRequest == true)
                {
                    interview.Availability = false;
                    db.Entry(interview).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("ListRequestedInterviews");
            }
            ViewBag.ApplicantID = new SelectList(db.Users, "Id", "FullName", applicantInterview.ApplicantID);
            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "RepresentativeID", applicantInterview.InterviewID);
            return View(applicantInterview);
        }

        // GET: ApplicantInterviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicantInterview applicantInterview = db.ApplicantInterviews.Find(id);
            if (applicantInterview == null)
            {
                return HttpNotFound();
            }
            return View(applicantInterview);
        }

        // POST: ApplicantInterviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicantInterview applicantInterview = db.ApplicantInterviews.Find(id);
            db.ApplicantInterviews.Remove(applicantInterview);
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
    }
}
