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
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace App.Controllers
{
    [Authorize(Roles = "Tech_Support,SrCollectors,Collectors")]
    public class Patient_Receivables_InfoController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Patient_Receivables_Info
        public ActionResult Index()
        {
            //var patient_Receivables_Info = db.Patient_Receivables_Info.Include(p => p.Patient_Master).Include(p => p.Patient_Type_Master).Include(p => p.Payer_Master);
            //return View(patient_Receivables_Info.ToList());
            return View();
        }
        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Patient_Receivables_Info.ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Patient_Receivables_Info> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                id = x.Patient_Id,
                typeCode = x.Patient_Type_Code,
                payerId = x.Payer_Id,
                socialSecurityNo = x.Patient_Master.Patient_SocialSecurity,
                name = x.Patient_Master.Patient_First_Name + " " + x.Patient_Master.Patient_Middle_Name + " " + x.Patient_Master.Patient_First_Name,
                patientTypeCode = x.Patient_Type_Code,
                payerName = x.Payer_Master.Payer_Name,
                patientNoFrmClient = x.Patient_Master.PatientNoFromClient,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Patient_Receivables_Info>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }
        // GET: Patient_Receivables_Info/Details/5
        public ActionResult Details(int id, string typecode, int payerId)
        {
            if (id == 0 || String.IsNullOrEmpty(typecode) || payerId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Receivables_Info patient_Receivables_Info = db.Patient_Receivables_Info.Find(id, typecode, payerId);
            if (patient_Receivables_Info == null)
            {
                return HttpNotFound();
            }
            return View(patient_Receivables_Info);
        }

        // GET: Patient_Receivables_Info/Create
        public ActionResult Create()
        {
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient");
            ViewBag.Patient_Type_Code = new SelectList(db.Patient_Type_Master, "Patient_Type_Code", "Patient_Type");
            ViewBag.Payer_Id = new SelectList(db.Payer_Master.Where(x=>x.IsActive), "Payer_Id", "Payer_Name");
            return View();
        }

        // POST: Patient_Receivables_Info/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Patient_Id,Patient_Type_Code,Payer_Id")] Patient_Receivables_Info patient_Receivables_Info)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    patient_Receivables_Info.Created_By_User = User.Identity.GetUserId<int>();
                    patient_Receivables_Info.Create_Dt_Time = dt;
                    patient_Receivables_Info.Updated_By_User = User.Identity.GetUserId<int>();
                    patient_Receivables_Info.Update_Dt_Time = dt;

                    db.Patient_Receivables_Info.Add(patient_Receivables_Info);
                    db.SaveChanges();
                }
                catch (DbUpdateException exUpdate)
                {
                    var sqlException = exUpdate.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        if (sqlException.Errors.Count > 0)
                        {
                            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Receivables_Info.Patient_Id);
                            ViewBag.Patient_Type_Code = new SelectList(db.Patient_Type_Master, "Patient_Type_Code", "Patient_Type", patient_Receivables_Info.Patient_Type_Code);
                            ViewBag.Payer_Id = new SelectList(db.Payer_Master.Where(x => x.IsActive), "Payer_Id", "Payer_Name", patient_Receivables_Info.Payer_Id);
                            switch (sqlException.Errors[0].Number)
                            {
                                case 547: // Foreign Key violation
                                    ModelState.AddModelError("CodeInUse", "Foreign key reference error");
                                    return View(patient_Receivables_Info);
                                case 2627:
                                    ModelState.AddModelError("Duplicate_key", "Patient receivable code already Exist in System");
                                    return View(patient_Receivables_Info);
                                default:
                                    throw;
                            }
                        }
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Receivables_Info.Patient_Id);
            ViewBag.Patient_Type_Code = new SelectList(db.Patient_Type_Master, "Patient_Type_Code", "Patient_Type", patient_Receivables_Info.Patient_Type_Code);
            ViewBag.Payer_Id = new SelectList(db.Payer_Master.Where(x => x.IsActive), "Payer_Id", "Payer_Name", patient_Receivables_Info.Payer_Id);
            return View(patient_Receivables_Info);
        }

        // GET: Patient_Receivables_Info/Edit/5
        public ActionResult Edit(int id, string typecode, int payerId)
        {
            if (id == 0 || String.IsNullOrEmpty(typecode) || payerId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Receivables_Info patient_Receivables_Info = db.Patient_Receivables_Info.Find(id, typecode, payerId);
            if (patient_Receivables_Info == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Receivables_Info.Patient_Id);
            ViewBag.Patient_Type_Code = new SelectList(db.Patient_Type_Master, "Patient_Type_Code", "Patient_Type", patient_Receivables_Info.Patient_Type_Code);
            ViewBag.Payer_Id = new SelectList(db.Payer_Master.Where(x => x.IsActive), "Payer_Id", "Payer_Name", patient_Receivables_Info.Payer_Id);
            return View(patient_Receivables_Info);
        }

        // POST: Patient_Receivables_Info/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Patient_Id,Patient_Type_Code,Payer_Id")] Patient_Receivables_Info patient_Receivables_Info)
        {
            if (ModelState.IsValid)
            {

                DateTime dt = DateTime.Now;
                patient_Receivables_Info.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Receivables_Info.Update_Dt_Time = dt;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                var patientMaster = db.Entry(patient_Receivables_Info);
                patientMaster.State = EntityState.Modified;
                foreach (var item in excluded)
                {
                    patientMaster.Property(item).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", patient_Receivables_Info.Patient_Id);
            ViewBag.Patient_Type_Code = new SelectList(db.Patient_Type_Master, "Patient_Type_Code", "Patient_Type", patient_Receivables_Info.Patient_Type_Code);
            ViewBag.Payer_Id = new SelectList(db.Payer_Master.Where(x => x.IsActive), "Payer_Id", "Payer_Name", patient_Receivables_Info.Payer_Id);
            return View(patient_Receivables_Info);
        }

        // GET: Patient_Receivables_Info/Delete/5
        public ActionResult Delete(int id, string typecode, int payerId)
        {
            if (id == 0 || String.IsNullOrEmpty(typecode) || payerId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Receivables_Info patient_Receivables_Info = db.Patient_Receivables_Info.Find(id, typecode, payerId);
            if (patient_Receivables_Info == null)
            {
                return HttpNotFound();
            }
            return View(patient_Receivables_Info);
        }

        // POST: Patient_Receivables_Info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string typecode, int payerId)
        {
            Patient_Receivables_Info patient_Receivables_Info = db.Patient_Receivables_Info.Find(id, typecode, payerId);
            db.Patient_Receivables_Info.Remove(patient_Receivables_Info);
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
