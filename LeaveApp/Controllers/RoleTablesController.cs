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
    public class RoleTablesController : Controller
    {
        private LeaveDBEntities db = new LeaveDBEntities();

        // GET: RoleTables
        public ActionResult Index()
        {
            var roleTables = db.RoleTables.Include(r => r.Teacher);
            return View(roleTables.ToList());
        }

        // GET: RoleTables/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTable roleTable = db.RoleTables.Find(id);
            if (roleTable == null)
            {
                return HttpNotFound();
            }
            return View(roleTable);
        }

        // GET: RoleTables/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name");
            return View();
        }

        // POST: RoleTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Password,UserRole")] RoleTable roleTable)
        {
            if (ModelState.IsValid)
            {
                db.RoleTables.Add(roleTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name", roleTable.ID);
            return View(roleTable);
        }

        // GET: RoleTables/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTable roleTable = db.RoleTables.Find(id);
            if (roleTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name", roleTable.ID);
            return View(roleTable);
        }

        // POST: RoleTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Password,UserRole")] RoleTable roleTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Teachers, "Id", "Name", roleTable.ID);
            return View(roleTable);
        }

        // GET: RoleTables/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleTable roleTable = db.RoleTables.Find(id);
            if (roleTable == null)
            {
                return HttpNotFound();
            }
            return View(roleTable);
        }

        // POST: RoleTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RoleTable roleTable = db.RoleTables.Find(id);
            db.RoleTables.Remove(roleTable);
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
