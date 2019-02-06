using PB2B.classes;
using PB2B.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB2B.Controllers.Admin
{
    [AdminAuthorize]
    public class ItemsController : Controller
    {
        LKSDBEntities1 lDb = new LKSDBEntities1();
        // GET: Items
        public ActionResult Index()
        {
            //var query = (from c in lDb.LG_006_ITEMS
            //             group c by c.STGRPCODE into g
            //             select new { MARKA = g.Key });

            //var model = query.Select(x=>x.MARKA.ToString()).OrderBy(x => x).ToList();
            //ViewBag.markalar = model;
            return View();
        }
        public PartialViewResult ItemListPartial(string searchParams)
        {
            var query = lDb.LG_006_ITEMS.AsQueryable();
            if (!String.IsNullOrEmpty(searchParams))
            {
                query = query.Where(x => (x.NAME + x.CODE + x.SPECODE).Replace(" ", "").Contains(searchParams.Replace(" ", "")));
            }

            return PartialView(query.OrderBy(x => x.NAME).ToList());
        }

        public ActionResult ItemProperties(string logicalref)
        {
            var model = lDb.LG_006_ITEMS.Find(Convert.ToInt32(logicalref));
            return View(model);
        }
        [HttpPost]
        public ActionResult ItemProperties(string logicalref, LG_006_ITEMS _model)
        {
            var model = lDb.LG_006_ITEMS.Find(Convert.ToInt32(logicalref));
            
            model.SPECODE = _model.SPECODE;
            model.SPECODE2 = _model.SPECODE2;
            model.SPECODE3 = _model.SPECODE3;
            model.SPECODE4 = _model.SPECODE4;
            model.SPECODE5 = _model.SPECODE5;
            lDb.LG_006_ITEMS.AddOrUpdate(model);
            lDb.SaveChanges();
            return View(model);
        }
        


    }
}