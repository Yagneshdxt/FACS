using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DbAccess;

namespace App.Controllers
{
    public class User_Type_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: User_Type_Master
        public ActionResult Index()
        {
            return View(db.User_Type_Master.ToList());
        }

        // GET: User_Type_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Type_Master user_Type_Master = db.User_Type_Master.Find(id);
            if (user_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(user_Type_Master);
        }

        // GET: User_Type_Master/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User_Type_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Type_Id,User_Type,Create_Dt_Time,Update_Dt_Time")] User_Type_Master user_Type_Master)
        {
            if (ModelState.IsValid)
            {
                db.User_Type_Master.Add(user_Type_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user_Type_Master);
        }

        // GET: User_Type_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Type_Master user_Type_Master = db.User_Type_Master.Find(id);
            if (user_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(user_Type_Master);
        }

        // POST: User_Type_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Type_Id,User_Type,Create_Dt_Time,Update_Dt_Time")] User_Type_Master user_Type_Master)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Type_Master).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_Type_Master);
        }

        // GET: User_Type_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Type_Master user_Type_Master = db.User_Type_Master.Find(id);
            if (user_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(user_Type_Master);
        }

        // POST: User_Type_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_Type_Master user_Type_Master = db.User_Type_Master.Find(id);
            db.User_Type_Master.Remove(user_Type_Master);
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
