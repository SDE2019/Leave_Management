using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeaveApp.Models;

namespace LeaveApp.Controllers
{
    public class FacultyController : Controller
    {
        private LeaveDBEntities db = new LeaveDBEntities();
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Leaves
        public ActionResult PreviousLeaves()
        {
            var leaves = db.Leaves.Include(l => l.Teacher);
            return View(leaves.ToList());
        }
        public ActionResult ApplyLeave()
        {
            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name");
            return View();
        }

        // POST: Leaves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ApplyLeave([Bind(Include = "LeaveID,ID,LeaveDescription,TempContact,StartDate,EndDate,LeaveType,LeaveTypeCount,TotalLeaveCount")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                leave.ID = User.Identity.Name;
                leave.Status = "Pending";
                db.Leaves.Add(leave);
                leave.LeaveTypeCount = leave.LeaveTypeCount + getdays(leave.StartDate, leave.EndDate);
                db.SaveChanges();
                return RedirectToAction("AppSuccess");
            }

            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name", leave.ID);
            return View(leave);
        }
        [Authorize]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        [Authorize]
        public ActionResult AppSuccess()
        { 
            return View();
        }


        [NonAction]
        public int getdays(System.DateTime start, System.DateTime end)
        {
            int count = (end - start).Days + 1;
            return count;
        }
    }
}
