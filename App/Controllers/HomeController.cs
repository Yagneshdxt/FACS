using DbAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private FACSDBEntities db = new FACSDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CommingSoon() {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region AjaxCalls

        public JsonResult getSubType(int? id) {
            
            List<Contact_Sub_Type_Master> conSubType = db.Contact_Sub_Type_Master.Where(x => x.Contact_Type_Id == id).ToList();
            return Json(new SelectList(conSubType, "Contact_Sub_Type_Id", "Contact_Sub_Type"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAddSubType(int? id)
        {

            List<Address_Sub_Type_Master> conSubType = db.Address_Sub_Type_Master.Where(x => x.Address_Type_Id == id).ToList();
            return Json(new SelectList(conSubType, "Address_Sub_Type_Id", "Address_Sub_Type"), JsonRequestBehavior.AllowGet);
        }

        #endregion
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