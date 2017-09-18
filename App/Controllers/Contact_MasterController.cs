using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DbAccess;
using System.Linq.Dynamic;
using Microsoft.AspNet.Identity;

namespace App.Controllers
{
    [Authorize(Roles = "Tech_Support")]
    public class Contact_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Contact_Master
        public ActionResult Index()
        {
            //var contact_Master = db.Contact_Master.Include(c => c.Contact_Sub_Type_Master).Include(c => c.Contact_Type_Master);
            //contact_Master.ToList();
            return View();
        }

        public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Contact_Master.Include(c => c.Contact_Sub_Type_Master).Include(c => c.Contact_Type_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Contact_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            string searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;
            var sortBy = "Contact_Person_Name";
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
                lst = lst.Where(x => x.Contact_Person_Name.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Contact_Id,
                x.Contact_Person_Name,
                x.Contact_Type_Master.Contact_Type,
                x.Contact_Sub_Type_Master.Contact_Sub_Type,
                created_at = x.Create_Dt_Time.Value.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.Value.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Contact_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }
        // GET: Contact_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact_Master contact_Master = db.Contact_Master.Find(id);
            if (contact_Master == null)
            {
                return HttpNotFound();
            }
            return View(contact_Master);
        }

        // GET: Contact_Master/Create
        public ActionResult Create()
        {
            var cntType = db.Contact_Type_Master;
            int cntTypeIdSelected = cntType.FirstOrDefault().Contact_Type_Id;
            ViewBag.Contact_Sub_Type = new SelectList(db.Contact_Sub_Type_Master.Where(x=>x.Contact_Type_Id == cntTypeIdSelected), "Contact_Sub_Type_Id", "Contact_Sub_Type");
            ViewBag.Contact_Type = new SelectList(cntType, "Contact_Type_Id", "Contact_Type", cntTypeIdSelected);
            return View();
        }

        // POST: Contact_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Contact_Type,Contact_Sub_Type,Country_Code,City_Code,Phone_1,Phone_2,Phone_3,Phone_4,Phone_5,Cell_1,Cell_2,Fax_1,Fax_2,Email_1,Email_2,Contact_Person_Name")] Contact_Master contact_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                contact_Master.Created_By_User = User.Identity.GetUserId<int>();
                contact_Master.Create_Dt_Time = dt;
                contact_Master.Updated_By_User = User.Identity.GetUserId<int>();
                contact_Master.Update_Dt_Time = dt;
                db.Contact_Master.Add(contact_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Contact_Sub_Type = new SelectList(db.Contact_Sub_Type_Master, "Contact_Sub_Type_Id", "Contact_Sub_Type", contact_Master.Contact_Sub_Type);
            ViewBag.Contact_Type = new SelectList(db.Contact_Type_Master, "Contact_Type_Id", "Contact_Type", contact_Master.Contact_Type);
            return View(contact_Master);
        }

        // GET: Contact_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact_Master contact_Master = db.Contact_Master.Find(id);
            if (contact_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.Contact_Sub_Type = new SelectList(db.Contact_Sub_Type_Master.Where(x=>x.Contact_Type_Id == contact_Master.Contact_Type), "Contact_Sub_Type_Id", "Contact_Sub_Type", contact_Master.Contact_Sub_Type);
            ViewBag.Contact_Type = new SelectList(db.Contact_Type_Master, "Contact_Type_Id", "Contact_Type", contact_Master.Contact_Type);
            return View(contact_Master);
        }

        // POST: Contact_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Contact_Id,Contact_Type,Contact_Sub_Type,Country_Code,City_Code,Phone_1,Phone_2,Phone_3,Phone_4,Phone_5,Cell_1,Cell_2,Fax_1,Fax_2,Email_1,Email_2,Contact_Person_Name")] Contact_Master contact_Master)
        {
            if (ModelState.IsValid)
            {
                Contact_Master cm = db.Contact_Master.Find(contact_Master.Contact_Id);
                cm.Contact_Type = contact_Master.Contact_Type;
                cm.Contact_Sub_Type = contact_Master.Contact_Sub_Type;
                cm.Country_Code = contact_Master.Country_Code;
                cm.City_Code = contact_Master.City_Code;
                cm.Phone_1 = contact_Master.Phone_1;
                cm.Phone_2 = contact_Master.Phone_2;
                cm.Phone_3 = contact_Master.Phone_3;
                cm.Phone_4 = contact_Master.Phone_4;
                cm.Phone_5 = contact_Master.Phone_5;
                cm.Cell_1 = contact_Master.Cell_1;
                cm.Cell_2 = contact_Master.Cell_2;
                cm.Fax_1 = contact_Master.Fax_1;
                cm.Fax_2 = contact_Master.Fax_2;
                cm.Email_1 = contact_Master.Email_1;
                cm.Email_2 = contact_Master.Email_2;
                cm.Contact_Person_Name = contact_Master.Contact_Person_Name;
                cm.Updated_By_User = User.Identity.GetUserId<int>();
                cm.Update_Dt_Time = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Contact_Sub_Type = new SelectList(db.Contact_Sub_Type_Master, "Contact_Sub_Type_Id", "Contact_Sub_Type", contact_Master.Contact_Sub_Type);
            ViewBag.Contact_Type = new SelectList(db.Contact_Type_Master, "Contact_Type_Id", "Contact_Type", contact_Master.Contact_Type);
            return View(contact_Master);
        }

        // GET: Contact_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact_Master contact_Master = db.Contact_Master.Find(id);
            if (contact_Master == null)
            {
                return HttpNotFound();
            }
            return View(contact_Master);
        }

        // POST: Contact_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact_Master contact_Master = db.Contact_Master.Find(id);
            db.Contact_Master.Remove(contact_Master);
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
