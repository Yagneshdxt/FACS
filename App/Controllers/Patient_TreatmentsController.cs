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
    public class Patient_TreatmentsController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Patient_Treatments
        public ActionResult Index()
        {
            //var patient_Treatments = db.Patient_Treatments.Include(p => p.Patient_Master);
            //return View(patient_Treatments.ToList());
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Patient_Treatments.Include(p => p.Patient_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Patient_Treatments> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                id = x.Patient_Treatments_Id,
                name = x.Patient_Master.Patient_First_Name + " " + x.Patient_Master.Patient_Middle_Name + " " + x.Patient_Master.Patient_Last_Name,
                patientNoFrmClient = x.Patient_Master.PatientNoFromClient,
                socialSecurityNo = x.Patient_Master.Patient_SocialSecurity,
                TreatmentCode = x.Treatment_Code,
                TreatmentDt = x.Treatment_Date.Value.ToString("MMM,dd,yyy"),
                TotalCharge = x.Total_Charges,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Patient_Treatments>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Patient_Treatments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Treatments patient_Treatments = db.Patient_Treatments.Find(id);
            if (patient_Treatments == null)
            {
                return HttpNotFound();
            }
            return View(patient_Treatments);
        }

        // GET: Patient_Treatments/Create
        public ActionResult Create()
        {
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient");
            return View();
        }

        // POST: Patient_Treatments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Patient_Id, Treatment_Date,Treatment_Code,Treatment_Description,Total_Charges")] Patient_Treatments patient_Treatments)
        {
            if (ModelState.IsValid)
            {
                 decimal? InsuranceAmt = db.Patient_Master.Where(x => x.Patient_Id == patient_Treatments.Patient_Id).Sum(x => x.Patient_Insurance_Bill_Amount)??0;

                 decimal? CollectedAmt = (db.Patient_Treatments.Where(x => x.Patient_Id == patient_Treatments.Patient_Id).Select(x =>  ((decimal?)x.Total_Charges))).Sum() ?? 0;


                if (InsuranceAmt < (CollectedAmt + patient_Treatments.Total_Charges))
                {
                    ModelState.AddModelError("ToatalExceed", String.Format("Total Charges {0}(including current charge) exceeds Insurance amount {1}", (CollectedAmt + patient_Treatments.Total_Charges), InsuranceAmt));
                    ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Treatments.Patient_Id);
                    return View(patient_Treatments);
                }

                DateTime dt = DateTime.Now;
                patient_Treatments.Created_By_User = User.Identity.GetUserId<int>();
                patient_Treatments.Create_Dt_Time = dt;
                patient_Treatments.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Treatments.Update_Dt_Time = dt;

                db.Patient_Treatments.Add(patient_Treatments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Treatments.Patient_Id);
            return View(patient_Treatments);
        }

        // GET: Patient_Treatments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Treatments patient_Treatments = db.Patient_Treatments.Find(id);
            if (patient_Treatments == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Treatments.Patient_Id);
            return View(patient_Treatments);
        }

        // POST: Patient_Treatments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Patient_Treatments_Id,Patient_Id,Treatment_Date,Treatment_Code,Treatment_Description,Total_Charges,Created_By_User,Updated_By_User,Create_Dt_Time,Update_Dt_Time")] Patient_Treatments patient_Treatments)
        {
            if (ModelState.IsValid)
            {
                decimal? InsuranceAmt = db.Patient_Master.Where(x => x.Patient_Id == patient_Treatments.Patient_Id).Sum(x => x.Patient_Insurance_Bill_Amount) ?? 0;

                decimal? CollectedAmt = (db.Patient_Treatments.Where(x => x.Patient_Id == patient_Treatments.Patient_Id).Select(x => ((decimal?)x.Total_Charges))).Sum() ?? 0;


                if (InsuranceAmt < (CollectedAmt + patient_Treatments.Total_Charges))
                {
                    ModelState.AddModelError("ToatalExceed", String.Format("Total Charges {0}(including current charge) exceeds Insurance amount {1}", (CollectedAmt + patient_Treatments.Total_Charges), InsuranceAmt));
                    ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Treatments.Patient_Id);
                    return View(patient_Treatments);
                }

                DateTime dt = DateTime.Now;
                patient_Treatments.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Treatments.Update_Dt_Time = dt;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                var patientMaster = db.Entry(patient_Treatments);
                patientMaster.State = EntityState.Modified;
                foreach (var item in excluded)
                {
                    patientMaster.Property(item).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Treatments.Patient_Id);
            return View(patient_Treatments);
        }

        // GET: Patient_Treatments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Treatments patient_Treatments = db.Patient_Treatments.Find(id);
            if (patient_Treatments == null)
            {
                return HttpNotFound();
            }
            return View(patient_Treatments);
        }

        // POST: Patient_Treatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient_Treatments patient_Treatments = db.Patient_Treatments.Find(id);
            db.Patient_Treatments.Remove(patient_Treatments);
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
