using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB2B.classes;
using PB2B.Entity;
namespace PB2B.Controllers.Admin
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        LOGOKAMPEntities1 db = new LOGOKAMPEntities1();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Plasiyerler()
        {
            var model = db.ZTbLUseR.Where(x => x.Yetkisi == 1).OrderBy(x => x.CariSTR).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Plasiyerler(ZTbLUseR _formCollection)
        {
            _formCollection.Yetkisi = 1;
            _formCollection.SipStr = _formCollection.Adi.Substring(0, 3);
            db.ZTbLUseR.Add(_formCollection);
            db.SaveChanges();
            var model = db.ZTbLUseR.Where(x => x.Yetkisi == 1).OrderBy(x => x.CariSTR).ToList();
            return View(model);
        }
        public ActionResult PlasiyerSil(string UserId) {
            var model = db.ZTbLUseR.Where(x => x.UserId.ToString() == UserId).SingleOrDefault();
            if (model!=null)
            {
                db.ZTbLUseR.Remove(model);
                db.SaveChanges();
                return RedirectToAction("Plasiyerler", "Admin");
            }
            return View();
        }
    }
}