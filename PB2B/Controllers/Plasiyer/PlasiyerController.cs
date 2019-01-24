using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB2B.classes;
using PB2B.Entity;
namespace PB2B.Controllers.Plasiyer
{
    [LoginAuthorize]
    public class PlasiyerController : Controller
    {
        LKSDBEntities1 tDb = new LKSDBEntities1();
        // GET: Plasiyer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PlasiyerIndex()
        {
            List<C_INT_CARIONEKRAN_006> model = new List<C_INT_CARIONEKRAN_006>();
            return View(model);
        }

        [HttpPost]
        public ActionResult PlasiyerIndex(string searchParam)
        {
            return View(tDb.C_INT_CARIONEKRAN_006.Where(x => (x.Firma_Bilgisi + x.KODU).Replace(" ", "").Contains(searchParam.Replace("", ""))).OrderBy(x => x.Firma_Bilgisi));
        }

        public ActionResult PlasiyerYonlendir(int id)
        {
            var model = tDb.C_INT_CARIONEKRAN_006.Where(x=>x.LOGREF==id).SingleOrDefault();
            if (model != null)
            {
                Session["Firma_Bilgisi"] = model.Firma_Bilgisi;
                Session["CODE"] = model.KODU;
                Session["CLIENTREF"] = model.LOGREF;
                return RedirectToAction("Index", "Musteri");
            }

            return RedirectToAction("Index");

        }
    }
}