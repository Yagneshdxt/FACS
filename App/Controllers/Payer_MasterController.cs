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
    public class Payer_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Payer_Master
        public ActionResult Index()
        {
            //db.Payer_Master.ToList()
            return View();
        }
        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Payer_Master.ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Payer_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "Payer_Name";
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
                lst = lst.Where(x => x.Payer_Name.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Payer_Id,
                x.Payer_Name,
                IsActive = x.IsActive ? "Active" : "Inactive",
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Payer_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }
        // GET: Payer_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payer_Master payer_Master = db.Payer_Master.Find(id);
            if (payer_Master == null)
            {
                return HttpNotFound();
            }
            return View(payer_Master);
        }

        // GET: Payer_Master/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payer_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Payer_Name,IsActive")] Payer_Master payer_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                payer_Master.Created_By_User = User.Identity.GetUserId<int>();
                payer_Master.Create_Dt_Time = dt;
                payer_Master.Updated_By_User = User.Identity.GetUserId<int>();
                payer_Master.Update_Dt_Time = dt;
                db.Payer_Master.Add(payer_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payer_Master);
        }

        // GET: Payer_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payer_Master payer_Master = db.Payer_Master.Find(id);
            if (payer_Master == null)
            {
                return HttpNotFound();
            }
            return View(payer_Master);
        }

        // POST: Payer_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Payer_Id,Payer_Name,IsActive")] Payer_Master payer_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                payer_Master.Updated_By_User = User.Identity.GetUserId<int>();
                payer_Master.Update_Dt_Time = dt;

                var payMst = db.Entry(payer_Master);
                payMst.State = EntityState.Modified;

                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                foreach (var item in excluded)
                {
                    payMst.Property(item).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payer_Master);
        }

        // GET: Payer_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payer_Master payer_Master = db.Payer_Master.Find(id);
            if (payer_Master == null)
            {
                return HttpNotFound();
            }
            return View(payer_Master);
        }

        // POST: Payer_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payer_Master payer_Master = db.Payer_Master.Find(id);
            db.Payer_Master.Remove(payer_Master);
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
