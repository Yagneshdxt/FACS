using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DbAccess;
using Microsoft.AspNet.Identity;
using System.Linq.Dynamic;

namespace App.Controllers
{
    [Authorize(Roles = "Tech_Support")]
    public class Disposition_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Disposition_Master
        public ActionResult Index()
        {
            //db.Disposition_Master.ToList();
            return View();
        }
        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Disposition_Master.ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Disposition_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "Disposition";
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
                lst = lst.Where(x => x.Disposition.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Disposition_Id,
                x.Disposition,
                x.Disposition_Group,
                IsActive = x.IsActive ? "Active" : "Inactive",
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Disposition_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Disposition_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disposition_Master disposition_Master = db.Disposition_Master.Find(id);
            if (disposition_Master == null)
            {
                return HttpNotFound();
            }
            return View(disposition_Master);
        }

        // GET: Disposition_Master/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disposition_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Disposition,Disposition_Group,IsActive")] Disposition_Master disposition_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                disposition_Master.Created_By_User = User.Identity.GetUserId<int>();
                disposition_Master.Create_Dt_Time = dt;
                disposition_Master.Updated_By_User = User.Identity.GetUserId<int>();
                disposition_Master.Update_Dt_Time = dt;
                db.Disposition_Master.Add(disposition_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(disposition_Master);
        }

        // GET: Disposition_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disposition_Master disposition_Master = db.Disposition_Master.Find(id);
            if (disposition_Master == null)
            {
                return HttpNotFound();
            }
            return View(disposition_Master);
        }

        // POST: Disposition_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Disposition_Id, Disposition,Disposition_Group,IsActive")] Disposition_Master disposition_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                disposition_Master.Updated_By_User = User.Identity.GetUserId<int>();
                disposition_Master.Update_Dt_Time = dt;

                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                var disMaster = db.Entry(disposition_Master);
                disMaster.State = EntityState.Modified;
                foreach (var item in excluded)
                {
                    disMaster.Property(item).IsModified = false;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(disposition_Master);
        }

        // GET: Disposition_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disposition_Master disposition_Master = db.Disposition_Master.Find(id);
            if (disposition_Master == null)
            {
                return HttpNotFound();
            }
            return View(disposition_Master);
        }

        // POST: Disposition_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Disposition_Master disposition_Master = db.Disposition_Master.Find(id);
            db.Disposition_Master.Remove(disposition_Master);
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
