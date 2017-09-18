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
    public class Patient_StatusController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Patient_Status
        public ActionResult Index()
        {
            //var patient_Status = db.Patient_Status.Include(p => p.Disposition_Master).Include(p => p.Patient_Master).Include(p=>p.Contact_Master);
            //return View(patient_Status.ToList());
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Patient_Status.Include(p => p.Disposition_Master).Include(p => p.Patient_Master).Include(p => p.Contact_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Patient_Status> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "socialSecurityNo";
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
                lst = lst.Where(x => x.Patient_Master.Patient_SocialSecurity.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Status_Id,
                socialSecurityNo = x.Patient_Master.Patient_SocialSecurity,
                patientNoFrmClient = x.Patient_Master.PatientNoFromClient,
                name = x.Patient_Master.Patient_First_Name + " " + x.Patient_Master.Patient_Middle_Name + " " + x.Patient_Master.Patient_Last_Name,
                Collector = x.Contact_Master.Contact_Person_Name,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Uppdate_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Patient_Status>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Patient_Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Status patient_Status = db.Patient_Status.Find(id);
            if (patient_Status == null)
            {
                return HttpNotFound();
            }
            return View(patient_Status);
        }

        // GET: Patient_Status/Create
        public ActionResult Create()
        {
            ViewBag.Disposition_Id = new SelectList(db.Disposition_Master, "Disposition_Id", "Disposition");
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient");
            ViewBag.Collector_Id = new SelectList(db.Contact_Master, "Contact_Id", "Contact_Person_Name");
            return View();
        }

        // POST: Patient_Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Patient_Id,Collector_Id,Disposition_Id,Contact_Date,Method_Of_Contact,Valid_Contact_Number,Valid_Contact_Number_Type,Current_Balance,Notes,IsLatest,Last_Payment_Date,Last_Payment_Amount,Total_Paid_Amount")] Patient_Status patient_Status)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                patient_Status.Created_By_User = User.Identity.GetUserId<int>();
                patient_Status.Create_Dt_Time = dt;
                patient_Status.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Status.Uppdate_Dt_Time = dt;

                db.Patient_Status.Add(patient_Status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Disposition_Id = new SelectList(db.Disposition_Master, "Disposition_Id", "Disposition", patient_Status.Disposition_Id);
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Status.Patient_Id);
            ViewBag.Collector_Id = new SelectList(db.Contact_Master, "Contact_Id", "Contact_Person_Name");
            return View(patient_Status);
        }

        // GET: Patient_Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Status patient_Status = db.Patient_Status.Find(id);
            if (patient_Status == null)
            {
                return HttpNotFound();
            }
            ViewBag.Disposition_Id = new SelectList(db.Disposition_Master, "Disposition_Id", "Disposition", patient_Status.Disposition_Id);
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Status.Patient_Id);
            ViewBag.Collector_Id = new SelectList(db.Contact_Master, "Contact_Id", "Contact_Person_Name", patient_Status.Collector_Id);
            return View(patient_Status);
        }

        // POST: Patient_Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Status_Id,Patient_Id,Collector_Id,Disposition_Id,Contact_Date,Method_Of_Contact,Valid_Contact_Number,Valid_Contact_Number_Type,Current_Balance,Notes,IsLatest,Last_Payment_Date,Last_Payment_Amount,Total_Paid_Amount")] Patient_Status patient_Status)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                patient_Status.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Status.Uppdate_Dt_Time = dt;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                var patientMaster = db.Entry(patient_Status);
                patientMaster.State = EntityState.Modified;
                foreach (var item in excluded)
                {
                    patientMaster.Property(item).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Disposition_Id = new SelectList(db.Disposition_Master, "Disposition_Id", "Disposition", patient_Status.Disposition_Id);
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Status.Patient_Id);
            ViewBag.Collector_Id = new SelectList(db.Contact_Master, "Contact_Id", "Contact_Person_Name", patient_Status.Collector_Id);
            return View(patient_Status);
        }

        // GET: Patient_Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Status patient_Status = db.Patient_Status.Find(id);
            if (patient_Status == null)
            {
                return HttpNotFound();
            }
            return View(patient_Status);
        }

        // POST: Patient_Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient_Status patient_Status = db.Patient_Status.Find(id);
            db.Patient_Status.Remove(patient_Status);
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
