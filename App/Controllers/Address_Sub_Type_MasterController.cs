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
    public class Address_Sub_Type_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Address_Sub_Type_Master
        public ActionResult Index()
        {
            //var address_Sub_Type_Master = db.Address_Sub_Type_Master.Include(a => a.Address_Type_Master);
            //address_Sub_Type_Master.ToList();
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Address_Sub_Type_Master.Include(a => a.Address_Type_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Address_Sub_Type_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "Address_Sub_Type";
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
                lst = lst.Where(x => x.Address_Sub_Type.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Address_Sub_Type_Id,
                x.Address_Sub_Type,
                x.Address_Type_Master.Address_Type,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Address_Sub_Type_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Address_Sub_Type_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address_Sub_Type_Master address_Sub_Type_Master = db.Address_Sub_Type_Master.Find(id);
            if (address_Sub_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(address_Sub_Type_Master);
        }

        // GET: Address_Sub_Type_Master/Create
        public ActionResult Create()
        {
            ViewBag.Address_Type_Id = new SelectList(db.Address_Type_Master, "Address_Type_Id", "Address_Type");
            return View();
        }

        // POST: Address_Sub_Type_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Address_Type_Id,Address_Sub_Type")] Address_Sub_Type_Master address_Sub_Type_Master)
        {
            if (ModelState.IsValid)
            {
                
                DateTime dt = DateTime.Now;
                address_Sub_Type_Master.Created_By_User = User.Identity.GetUserId<int>();
                address_Sub_Type_Master.Create_Dt_Time = dt;
                address_Sub_Type_Master.Updated_By_User = User.Identity.GetUserId<int>();
                address_Sub_Type_Master.Update_Dt_Time = dt;
                db.Address_Sub_Type_Master.Add(address_Sub_Type_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Address_Type_Id = new SelectList(db.Address_Type_Master, "Address_Type_Id", "Address_Type", address_Sub_Type_Master.Address_Type_Id);
            return View(address_Sub_Type_Master);
        }

        // GET: Address_Sub_Type_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address_Sub_Type_Master address_Sub_Type_Master = db.Address_Sub_Type_Master.Find(id);
            if (address_Sub_Type_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.Address_Type_Id = new SelectList(db.Address_Type_Master, "Address_Type_Id", "Address_Type", address_Sub_Type_Master.Address_Type_Id);
            return View(address_Sub_Type_Master);
        }

        // POST: Address_Sub_Type_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Address_Sub_Type_Id,Address_Type_Id,Address_Sub_Type")] Address_Sub_Type_Master address_Sub_Type_Master)
        {
            if (ModelState.IsValid)
            {
                address_Sub_Type_Master.Updated_By_User = User.Identity.GetUserId<int>();
                address_Sub_Type_Master.Update_Dt_Time = DateTime.Now;

                var addSubType = db.Entry(address_Sub_Type_Master);
                addSubType.State = EntityState.Modified;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                foreach (var item in excluded)
                {
                    addSubType.Property(item).IsModified = false;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Address_Type_Id = new SelectList(db.Address_Type_Master, "Address_Type_Id", "Address_Type", address_Sub_Type_Master.Address_Type_Id);
            return View(address_Sub_Type_Master);
        }

        // GET: Address_Sub_Type_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address_Sub_Type_Master address_Sub_Type_Master = db.Address_Sub_Type_Master.Find(id);
            if (address_Sub_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(address_Sub_Type_Master);
        }

        // POST: Address_Sub_Type_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address_Sub_Type_Master address_Sub_Type_Master = db.Address_Sub_Type_Master.Find(id);
            db.Address_Sub_Type_Master.Remove(address_Sub_Type_Master);
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
