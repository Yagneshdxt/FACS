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
    public class Address_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Address_Master
        public ActionResult Index()
        {
            //var address_Master = db.Address_Master.Include(a => a.Address_Sub_Type_Master);
            //address_Master.ToList()
            return View();
        }

         public JsonResult GetJsonList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var jsonLst = db.Address_Master.Include(a => a.Address_Sub_Type_Master.Address_Type_Master).ToList();

            var GetList = GetDatTableLst(jsonLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetDatTableLst(List<Address_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                lst = lst.Where(x => x.Address_Sub_Type_Master.Address_Sub_Type.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Address_Id,
                x.Address_Sub_Type_Master.Address_Type_Master.Address_Type,
                x.Address_Sub_Type_Master.Address_Sub_Type,
                created_at = x.Create_Dt_Time.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Address_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Address_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address_Master address_Master = db.Address_Master.Find(id);
            if (address_Master == null)
            {
                return HttpNotFound();
            }
            return View(address_Master);
        }

        // GET: Address_Master/Create
        public ActionResult Create()
        {
            var AddType = db.Address_Type_Master;
            int AddTypeSele = AddType.FirstOrDefault().Address_Type_Id;
            ViewBag.Address_Type = new SelectList(AddType, "Address_Type_Id", "Address_Type", AddTypeSele);
            ViewBag.Address_Sub_Type = new SelectList(db.Address_Sub_Type_Master.Where(z=>z.Address_Type_Id == AddTypeSele), "Address_Sub_Type_Id", "Address_Sub_Type");
            return View();
        }

        // POST: Address_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Address_Type,Address_Sub_Type,Address_Line_1,Address_Line_2,Address_Line_3,City,State,ZipCode")] Address_Master address_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                address_Master.Created_By_User = User.Identity.GetUserId<int>();
                address_Master.Create_Dt_Time = dt;
                address_Master.Updated_By_User = User.Identity.GetUserId<int>();
                address_Master.Update_Dt_Time = dt;
                
                db.Address_Master.Add(address_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Address_Type = new SelectList(db.Address_Type_Master, "Address_Type_Id", "Address_Type", address_Master.Address_Type);
            ViewBag.Address_Sub_Type = new SelectList(db.Address_Sub_Type_Master.Where(x=>x.Address_Type_Id == address_Master.Address_Type), "Address_Sub_Type_Id", "Address_Sub_Type", address_Master.Address_Sub_Type);
            return View(address_Master);
        }

        // GET: Address_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address_Master address_Master = db.Address_Master.Include(x=>x.Address_Sub_Type_Master.Address_Type_Master).Where(x=>x.Address_Id == id).FirstOrDefault();
            if (address_Master == null)
            {
                return HttpNotFound();
            }

            var addSubType = db.Address_Sub_Type_Master;
            int addTypIdSel = addSubType.Where(x => x.Address_Sub_Type_Id == address_Master.Address_Sub_Type).FirstOrDefault().Address_Type_Id;
            ViewBag.Address_Type = new SelectList(db.Address_Type_Master, "Address_Type_Id", "Address_Type", addTypIdSel);
            ViewBag.Address_Sub_Type = new SelectList(addSubType.Where(x=>x.Address_Type_Id == addTypIdSel), "Address_Sub_Type_Id", "Address_Sub_Type", address_Master.Address_Sub_Type);
            return View(address_Master);
        }

        // POST: Address_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Address_Id,Address_Type,Address_Sub_Type,Address_Line_1,Address_Line_2,Address_Line_3,City,State,ZipCode")] Address_Master address_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                address_Master.Updated_By_User = User.Identity.GetUserId<int>();
                address_Master.Update_Dt_Time = dt;

                var addMaster = db.Entry(address_Master);
                addMaster.State = EntityState.Modified;
                var excluded = new[] { "Created_By_User", "Create_Dt_Time" };
                foreach (var item in excluded)
                {
                    addMaster.Property(item).IsModified = false;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Address_Sub_Type = new SelectList(db.Address_Sub_Type_Master, "Address_Sub_Type_Id", "Address_Sub_Type", address_Master.Address_Sub_Type);
            return View(address_Master);
        }

        // GET: Address_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address_Master address_Master = db.Address_Master.Find(id);
            if (address_Master == null)
            {
                return HttpNotFound();
            }
            return View(address_Master);
        }

        // POST: Address_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address_Master address_Master = db.Address_Master.Find(id);
            db.Address_Master.Remove(address_Master);
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
