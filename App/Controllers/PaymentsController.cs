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
    public class PaymentsController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Payments
        public ActionResult Index()
        {
            //var payments = db.Payments.Include(p => p.Patient_Master);
            //return View(payments.ToList());
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Payments.Include(p => p.Patient_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Payment> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                id = x.Payments_Id,
                name = x.Patient_Master.Patient_First_Name + " " + x.Patient_Master.Patient_Middle_Name + " " + x.Patient_Master.Patient_Last_Name,
                patientNoFrmClient = x.Patient_Master.PatientNoFromClient,
                socialSecurityNo = x.Patient_Master.Patient_SocialSecurity,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Payment>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Patient_Id,Payment_Index,Payment_Type,Payment_Amount,Payment_Date,Payment_Post_Date,Payment_Details,IsActive,Revenue")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                payment.Created_By_User = User.Identity.GetUserId<int>();
                payment.Create_Dt_Time = dt;
                payment.Updated_By_User = User.Identity.GetUserId<int>();
                payment.Update_Dt_Time = dt;

                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", payment.Patient_Id);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", payment.Patient_Id);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Payments_Id,Patient_Id,Payment_Index,Payment_Type,Payment_Amount,Payment_Date,Payment_Post_Date,Payment_Details,IsActive,Revenue")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                payment.Updated_By_User = User.Identity.GetUserId<int>();
                payment.Update_Dt_Time = dt;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                var patientMaster = db.Entry(payment);
                patientMaster.State = EntityState.Modified;
                foreach (var item in excluded)
                {
                    patientMaster.Property(item).IsModified = false;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Patient_Id = new SelectList(db.Patient_Master, "Patient_Id", "PatientNoFromClient", payment.Patient_Id);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
