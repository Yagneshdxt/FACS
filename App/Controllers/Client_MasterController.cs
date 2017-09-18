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
    public class Client_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Client_Master
        public ActionResult Index()
        {
            //var client_Master = db.Client_Master.Include(c => c.Client_Group_Master);
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Client_Master.Include(c => c.Client_Group_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Client_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "Hospital_Name";
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
                lst = lst.Where(x => x.Hospital_Name.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Hospital_Id,
                x.Hospital_Name,
                x.Hospital_Speciality,
                x.Client_Group_Master.Hospital_Group_Name,
                IsActive = x.IsActive?"Active":"Inactive",
                created_at = x.Create_Dt_Time.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Client_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Client_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Master client_Master = db.Client_Master.Find(id);
            if (client_Master == null)
            {
                return HttpNotFound();
            }
            return View(client_Master);
        }

        // GET: Client_Master/Create
        public ActionResult Create()
        {
            ViewBag.Hospital_Group_Id = new SelectList(db.Client_Group_Master, "Hospital_Group_Id", "Hospital_Group_Name");
            return View();
        }

        // POST: Client_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Hospital_Name,Hospital_Group_Id,Hospital_Speciality,Number_Of_Beds,ICCU_Available,Visitor_Hours,IsActive")] Client_Master client_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                client_Master.Create_By_User = User.Identity.GetUserId<int>();
                client_Master.Updated_By_User = User.Identity.GetUserId<int>();
                client_Master.Create_Dt_Time = dt;
                client_Master.Update_Dt_Time = dt;
                db.Client_Master.Add(client_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Hospital_Group_Id = new SelectList(db.Client_Group_Master, "Hospital_Group_Id", "Hospital_Group_Name", client_Master.Hospital_Group_Id);
            return View(client_Master);
        }

        // GET: Client_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Master client_Master = db.Client_Master.Find(id);
            if (client_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hospital_Group_Id = new SelectList(db.Client_Group_Master, "Hospital_Group_Id", "Hospital_Group_Name", client_Master.Hospital_Group_Id);
            return View(client_Master);
        }

        // POST: Client_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Hospital_Id,Hospital_Name,Hospital_Group_Id,Hospital_Speciality,Number_Of_Beds,ICCU_Available,Visitor_Hours,IsActive")] Client_Master client_Master)
        {
            if (ModelState.IsValid)
            {
                Client_Master dbClMst = db.Client_Master.Find(client_Master.Hospital_Id);
                dbClMst.Hospital_Name = client_Master.Hospital_Name;
                dbClMst.Hospital_Group_Id = client_Master.Hospital_Group_Id;
                dbClMst.Hospital_Speciality = client_Master.Hospital_Speciality;
                dbClMst.Number_Of_Beds = client_Master.Number_Of_Beds;
                dbClMst.ICCU_Available = client_Master.ICCU_Available;
                dbClMst.Visitor_Hours = client_Master.Visitor_Hours;
                dbClMst.IsActive = client_Master.IsActive;
                dbClMst.Updated_By_User = User.Identity.GetUserId<int>();
                dbClMst.Update_Dt_Time = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Hospital_Group_Id = new SelectList(db.Client_Group_Master, "Hospital_Group_Id", "Hospital_Group_Name", client_Master.Hospital_Group_Id);
            return View(client_Master);
        }

        // GET: Client_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Master client_Master = db.Client_Master.Find(id);
            if (client_Master == null)
            {
                return HttpNotFound();
            }
            return View(client_Master);
        }

        // POST: Client_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client_Master client_Master = db.Client_Master.Find(id);
            db.Client_Master.Remove(client_Master);
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
