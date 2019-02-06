using PB2B.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB2B.Controllers.Admin
{
    public class IndirimlerController : Controller
    {
        LOGOKAMPEntities1 db = new LOGOKAMPEntities1();
        // GET: Indirimler
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GrupIndirimList()
        {
            var model = db.IND_COND.OrderByDescending(x => x.LOGICALREF).ToList();
            return View(model);
        }


        public ActionResult CreateGrupIndirim()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGrupIndirim(IND_COND _model)
        {
            db.IND_COND.AddOrUpdate(_model);
            db.SaveChanges();
            return RedirectToAction("GrupIndirimList");
        }
        public ActionResult EditGrupIndirim(string id)
        {
            var model = db.IND_COND.Find(Convert.ToInt32(id));

            return View(model);
        }
        public ActionResult DeleteGrupIndirim(string id)
        {
            var model = db.IND_COND.Find(Convert.ToInt32(id));
            db.IND_COND.Remove(model);
            db.SaveChanges();
            return RedirectToAction("GrupIndirimList");
        }

        [HttpPost]
        public ActionResult EditGrupIndirim(string id, IND_COND _model)
        {
            var model = db.IND_COND.Find(Convert.ToInt32(id));
            model.COND1 = _model.COND1;
            model.COND2 = _model.COND2;
            model.COND3 = _model.COND3;
            model.ORAN = _model.ORAN;
            model.INDORAN = _model.INDORAN;
            model.ALTORAN = _model.ALTORAN;
            db.IND_COND.AddOrUpdate(model);
            db.SaveChanges();
            return RedirectToAction("GrupIndirimList");
        }
    }
}