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
    public class Client_Group_MasterController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();

        // GET: Client_Group_Master
        public ActionResult Index()
        {
            //db.Client_Group_Master.ToList()
            return View();
        }

        public JsonResult GetclientGrpList(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var subCatLst = db.Client_Group_Master.ToList();

            var GetList = GetClintGrpDatTableLst(subCatLst, model, out filteredResultsCount, out totalResultsCount);

            return Json(new
            {
                data = GetList,
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
            }, JsonRequestBehavior.AllowGet);
        }

        private object GetClintGrpDatTableLst(List<Client_Group_Master> lst, DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                lst = lst.Where(x => x.Hospital_Group_Name.ToLower().Contains(searchBy.ToLower())).ToList();
            }

            filteredResultsCount = lst.Count();
            lst = lst.Skip(skip).Take(take).ToList();

            var lstCityformate = lst.Select(x => new
            {
                id = x.Hospital_Group_Id,
                x.Hospital_Group_Name,
                x.Hospital_Group_Code,
                created_at = x.Create_Dt_Time.ToString("MMM,dd,yyy"),
                updated_at = x.Update_Dt_Time.ToString("MMM,dd,yyy")
            });

            if (lst == null)
            {
                // empty collection...
                return new List<Client_Group_Master>();
            }
            lstCityformate = lstCityformate.OrderBy(sortBy + " " + sortDir).ToList();
            return lstCityformate;
        }

        // GET: Client_Group_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Group_Master client_Group_Master = db.Client_Group_Master.Find(id);
            if (client_Group_Master == null)
            {
                return HttpNotFound();
            }
            return View(client_Group_Master);
        }

        // GET: Client_Group_Master/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Client_Group_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Hospital_Group_Name,Hospital_Group_Code,Commission_Percentage")] Client_Group_Master client_Group_Master)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                client_Group_Master.Created_By_User = User.Identity.GetUserId<int>();
                client_Group_Master.Updated_By_User = User.Identity.GetUserId<int>();
                client_Group_Master.Create_Dt_Time = dt;
                client_Group_Master.Update_Dt_Time = dt;
                db.Client_Group_Master.Add(client_Group_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client_Group_Master);
        }

        // GET: Client_Group_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Group_Master client_Group_Master = db.Client_Group_Master.Find(id);
            if (client_Group_Master == null)
            {
                return HttpNotFound();
            }
            return View(client_Group_Master);
        }

        // POST: Client_Group_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Hospital_Group_Id,Hospital_Group_Name,Hospital_Group_Code,Commission_Percentage")] Client_Group_Master client_Group_Master)
        {
            if (ModelState.IsValid)
            {
                Client_Group_Master dbClientGrpMst = db.Client_Group_Master.Find(client_Group_Master.Hospital_Group_Id);
                dbClientGrpMst.Hospital_Group_Name = client_Group_Master.Hospital_Group_Name;
                dbClientGrpMst.Hospital_Group_Code = client_Group_Master.Hospital_Group_Code;
                dbClientGrpMst.Commission_Percentage = client_Group_Master.Commission_Percentage;
                dbClientGrpMst.Updated_By_User = User.Identity.GetUserId<int>();
                dbClientGrpMst.Update_Dt_Time = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client_Group_Master);
        }

        // GET: Client_Group_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client_Group_Master client_Group_Master = db.Client_Group_Master.Find(id);
            if (client_Group_Master == null)
            {
                return HttpNotFound();
            }
            return View(client_Group_Master);
        }

        // POST: Client_Group_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client_Group_Master client_Group_Master = db.Client_Group_Master.Find(id);
            db.Client_Group_Master.Remove(client_Group_Master);
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
