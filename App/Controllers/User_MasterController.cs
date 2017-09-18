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
    public class User_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: User_Master
        public ActionResult Index()
        {
            var user_Master = db.User_Master.Include(u => u.User_Type_Master);
            return View(user_Master.ToList());
        }

        // GET: User_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Master user_Master = db.User_Master.Find(id);
            if (user_Master == null)
            {
                return HttpNotFound();
            }
            return View(user_Master);
        }

        // GET: User_Master/Create
        public ActionResult Create()
        {
            ViewBag.User_Type_Id = new SelectList(db.User_Type_Master, "User_Type_Id", "User_Type");
            return View();
        }

        // POST: User_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "User_Id,User_Name,First_Name,Last_Name,Employee_Code,User_Type_Id,Email,Phone_Extention,Created_By_User,Updated_By_User,Create_Dt_Time,Update_Dt_Time,IsActive")] User_Master user_Master)
        {
            if (ModelState.IsValid)
            {
                db.User_Master.Add(user_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_Type_Id = new SelectList(db.User_Type_Master, "User_Type_Id", "User_Type", user_Master.User_Type_Id);
            return View(user_Master);
        }

        // GET: User_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Master user_Master = db.User_Master.Find(id);
            if (user_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Type_Id = new SelectList(db.User_Type_Master, "User_Type_Id", "User_Type", user_Master.User_Type_Id);
            return View(user_Master);
        }

        // POST: User_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_Id,User_Name,First_Name,Last_Name,Employee_Code,User_Type_Id,Email,Phone_Extention,Created_By_User,Updated_By_User,Create_Dt_Time,Update_Dt_Time,IsActive")] User_Master user_Master)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Master).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Type_Id = new SelectList(db.User_Type_Master, "User_Type_Id", "User_Type", user_Master.User_Type_Id);
            return View(user_Master);
        }

        // GET: User_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Master user_Master = db.User_Master.Find(id);
            if (user_Master == null)
            {
                return HttpNotFound();
            }
            return View(user_Master);
        }

        // POST: User_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_Master user_Master = db.User_Master.Find(id);
            db.User_Master.Remove(user_Master);
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
