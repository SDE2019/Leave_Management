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

        // GET: Leaves
        public ActionResult Index()
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
        public ActionResult ApplyLeave([Bind(Include = "LeaveID,ID,LeaveDescription,TempContact,StartDate,EndDate,LeaveType,LeaveTypeCount,TotalLeaveCount")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Leaves.Add(leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name", leave.ID);
            return View(leave);
        }
    }
}
