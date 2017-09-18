using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DbAccess;
using System.Linq.Dynamic;
using Microsoft.AspNet.Identity;

namespace App.Controllers
{
    [Authorize(Roles = "Tech_Support")]
    public class Contact_Sub_Type_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Contact_Sub_Type_Master
        public ActionResult Index()
        {
            //var contact_Sub_Type_Master = db.Contact_Sub_Type_Master.Include(c => c.Contact_Type_Master);
            //contact_Sub_Type_Master.ToList();
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Contact_Sub_Type_Master.Include(c => c.Contact_Type_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Contact_Sub_Type_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "Contact_Sub_Type";
            string sortDir = "ASC";

            totalResultsCount = lst.Count();

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc" ? "ASC" : "DESC";
            }
            if (!String.IsNullOrEmpty(searchBy))
            {
                lst = lst.Where(x => x.Contact_Sub_Type.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Contact_Sub_Type_Id,
                x.Contact_Sub_Type,
                x.Contact_Type_Master.Contact_Type,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Contact_Sub_Type_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Contact_Sub_Type_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact_Sub_Type_Master contact_Sub_Type_Master = db.Contact_Sub_Type_Master.Find(id);
            if (contact_Sub_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(contact_Sub_Type_Master);
        }

        // GET: Contact_Sub_Type_Master/Create
        public ActionResult Create()
        {
            ViewBag.Contact_Type_Id = new SelectList(db.Contact_Type_Master, "Contact_Type_Id", "Contact_Type");
            return View();
        }

        // POST: Contact_Sub_Type_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Contact_Type_Id,Contact_Sub_Type")] Contact_Sub_Type_Master contact_Sub_Type_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                contact_Sub_Type_Master.Created_By_User = User.Identity.GetUserId<int>();
                contact_Sub_Type_Master.Create_Dt_Time = dt;
                contact_Sub_Type_Master.Updated_By_User = User.Identity.GetUserId<int>();
                contact_Sub_Type_Master.Update_Dt_Time = dt;
                db.Contact_Sub_Type_Master.Add(contact_Sub_Type_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Contact_Type_Id = new SelectList(db.Contact_Type_Master, "Contact_Type_Id", "Contact_Type", contact_Sub_Type_Master.Contact_Type_Id);
            return View(contact_Sub_Type_Master);
        }

        // GET: Contact_Sub_Type_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact_Sub_Type_Master contact_Sub_Type_Master = db.Contact_Sub_Type_Master.Find(id);
            if (contact_Sub_Type_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.Contact_Type_Id = new SelectList(db.Contact_Type_Master, "Contact_Type_Id", "Contact_Type", contact_Sub_Type_Master.Contact_Type_Id);
            return View(contact_Sub_Type_Master);
        }

        // POST: Contact_Sub_Type_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Contact_Sub_Type_Id,Contact_Type_Id,Contact_Sub_Type")] Contact_Sub_Type_Master contact_Sub_Type_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                Contact_Sub_Type_Master cst = db.Contact_Sub_Type_Master.Find(contact_Sub_Type_Master.Contact_Sub_Type_Id);
                cst.Contact_Type_Id = contact_Sub_Type_Master.Contact_Type_Id;
                cst.Contact_Sub_Type = contact_Sub_Type_Master.Contact_Sub_Type;
                contact_Sub_Type_Master.Updated_By_User = User.Identity.GetUserId<int>();
                contact_Sub_Type_Master.Update_Dt_Time = dt;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Contact_Type_Id = new SelectList(db.Contact_Type_Master, "Contact_Type_Id", "Contact_Type", contact_Sub_Type_Master.Contact_Type_Id);
            return View(contact_Sub_Type_Master);
        }

        // GET: Contact_Sub_Type_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact_Sub_Type_Master contact_Sub_Type_Master = db.Contact_Sub_Type_Master.Find(id);
            if (contact_Sub_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(contact_Sub_Type_Master);
        }

        // POST: Contact_Sub_Type_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact_Sub_Type_Master contact_Sub_Type_Master = db.Contact_Sub_Type_Master.Find(id);
            db.Contact_Sub_Type_Master.Remove(contact_Sub_Type_Master);
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
