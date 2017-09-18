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
    [Authorize(Roles = "Tech_Support,SrCollectors,Collectors")]
    public class Patient_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Patient_Master
        public ActionResult Index()
        {
            //db.Patient_Master.ToList()
            return View();
        }
        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Patient_Master.ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Patient_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "name";
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
                lst = lst.Where(x => x.Patient_First_Name.ToLower().Contains(searchBy.ToLower()) || x.PatientNoFromClient.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Patient_Id,
                name = x.Patient_First_Name + " " + x.Patient_Middle_Name + " " + x.Patient_Last_Name,
                patientNo = x.PatientNoFromClient,
                ssnNo = x.Patient_SocialSecurity,
                IsActive = x.IsActive ? "Active" : "Inactive",
                created_at = x.Create_Dt_Time.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Patient_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Patient_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Master patient_Master = db.Patient_Master.Find(id);
            if (patient_Master == null)
            {
                return HttpNotFound();
            }
            return View(patient_Master);
        }

        // GET: Patient_Master/Create
        public ActionResult Create()
        {
            
            ViewBag.Hospital_Id = new SelectList(db.Client_Master.Where(x => x.IsActive), "Hospital_Id", "Hospital_Name"); 
            return View();
        }

        // POST: Patient_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientNoFromClient,Patient_Last_Name,Patient_First_Name,Patient_Middle_Name,Patient_SocialSecurity,Hospital_Id,Admit_Date,Discharge_Date,Patient_Bill_Amount,Patient_Insurance_Bill_Amount,Total_Charges,Placement_Date,IsActive,IsClosed,Closed_Date")] Patient_Master patient_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                patient_Master.Created_By_User = User.Identity.GetUserId<int>();
                patient_Master.Create_Dt_Time = dt;
                patient_Master.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Master.Update_Dt_Time = dt;
                db.Patient_Master.Add(patient_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient_Master);
        }

        // GET: Patient_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Master patient_Master = db.Patient_Master.Find(id);
            if (patient_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hospital_Id = new SelectList(db.Client_Master.Where(x => x.IsActive), "Hospital_Id", "Hospital_Name", patient_Master.Hospital_Id);
            return View(patient_Master);
        }

        // POST: Patient_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Patient_Id,PatientNoFromClient,Patient_Last_Name,Patient_First_Name,Patient_Middle_Name,Patient_SocialSecurity,Hospital_Id,Admit_Date,Discharge_Date,Patient_Bill_Amount,Patient_Insurance_Bill_Amount,Total_Charges,Placement_Date,IsActive,IsClosed,Closed_Date")] Patient_Master patient_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                patient_Master.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Master.Update_Dt_Time = dt;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                var patientMaster =  db.Entry(patient_Master);
                patientMaster.State = EntityState.Modified;
                foreach (var item in excluded)
                {
                    patientMaster.Property(item).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient_Master);
        }

        // GET: Patient_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Master patient_Master = db.Patient_Master.Find(id);
            if (patient_Master == null)
            {
                return HttpNotFound();
            }
            return View(patient_Master);
        }

        // POST: Patient_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient_Master patient_Master = db.Patient_Master.Find(id);
            db.Patient_Master.Remove(patient_Master);
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
