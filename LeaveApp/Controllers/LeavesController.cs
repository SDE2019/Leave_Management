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
    public class LeavesController : Controller
    {
        private LeaveDBEntities db = new LeaveDBEntities();

        // GET: Leaves
        public ActionResult Index()
        {
            var leaves = db.Leaves.Include(l => l.Teacher);
            return View(leaves.ToList());
        }

        // GET: Leaves/Details/5
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

        // GET: Leaves/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name");
            return View();
        }

        // POST: Leaves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeaveID,ID,LeaveDescription,TempContact,StartDate,EndDate,LeaveType,LeaveTypeCount,TotalLeaveCount")] Leave leave)
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

        // GET: Leaves/Edit/5
        public ActionResult Edit(long? id)
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
            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name", leave.ID);
            return View(leave);
        }

        // POST: Leaves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeaveID,ID,LeaveDescription,TempContact,StartDate,EndDate,LeaveType,LeaveTypeCount,TotalLeaveCount")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name", leave.ID);
            return View(leave);
        }

        // GET: Leaves/Delete/5
        public ActionResult Delete(long? id)
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
       
        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Leave leave = db.Leaves.Find(id);
            db.Leaves.Remove(leave);
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
