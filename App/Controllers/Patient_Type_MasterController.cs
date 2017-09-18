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
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace App.Controllers
{
    [Authorize(Roles = "Tech_Support")]
    public class Patient_Type_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Patient_Type_Master
        public ActionResult Index()
        {
            ///var patient_Type_Master = db.Patient_Type_Master.ToList();
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Patient_Type_Master.ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Patient_Type_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                lst = lst.Where(x => x.Patient_Type.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Patient_Type_Code,
                name = x.Patient_Type,
                patient_type_code = x.Patient_Type_Code,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Patient_Type_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Patient_Type_Master/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Type_Master patient_Type_Master = db.Patient_Type_Master.Find(id);
            if (patient_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(patient_Type_Master);
        }

        // GET: Patient_Type_Master/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient_Type_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Patient_Type_Code,Patient_Type")] Patient_Type_Master patient_Type_Master)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime dt = DateTime.Now;
                    patient_Type_Master.Created_By_User = User.Identity.GetUserId<int>();
                    patient_Type_Master.Create_Dt_Time = dt;
                    patient_Type_Master.Updated_By_User = User.Identity.GetUserId<int>();
                    patient_Type_Master.Update_Dt_Time = dt;
                    db.Patient_Type_Master.Add(patient_Type_Master);
                    db.SaveChanges();
                }
                catch (DbUpdateException exUpdate)
                {
                    var sqlException = exUpdate.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        if (sqlException.Errors.Count > 0)
                        {
                            switch (sqlException.Errors[0].Number)
                            {
                                case 547: // Foreign Key violation
                                    ModelState.AddModelError("CodeInUse", "Foreign key reference error");
                                    return View(patient_Type_Master);
                                case 2627:
                                    ModelState.AddModelError("Patient_Type_Code", "Patient Type Code already Exist in System");
                                    return View(patient_Type_Master);
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

            return View(patient_Type_Master);
        }

        // GET: Patient_Type_Master/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Type_Master patient_Type_Master = db.Patient_Type_Master.Find(id);
            if (patient_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(patient_Type_Master);
        }

        // POST: Patient_Type_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Patient_Type_Code,Patient_Type")] Patient_Type_Master patient_Type_Master)
        {
            if (ModelState.IsValid)
            {

                DateTime dt = DateTime.Now;
                patient_Type_Master.Updated_By_User = User.Identity.GetUserId<int>();
                patient_Type_Master.Update_Dt_Time = dt;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                var patientMaster = db.Entry(patient_Type_Master);
                patientMaster.State = EntityState.Modified;
                foreach (var item in excluded)
                {
                    patientMaster.Property(item).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient_Type_Master);
        }

        // GET: Patient_Type_Master/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient_Type_Master patient_Type_Master = db.Patient_Type_Master.Find(id);
            if (patient_Type_Master == null)
            {
                return HttpNotFound();
            }
            return View(patient_Type_Master);
        }

        // POST: Patient_Type_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Patient_Type_Master patient_Type_Master = db.Patient_Type_Master.Find(id);
            db.Patient_Type_Master.Remove(patient_Type_Master);
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
