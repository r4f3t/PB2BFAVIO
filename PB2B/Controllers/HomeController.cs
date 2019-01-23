using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB2B.Entity;
namespace PB2B.Controllers
{
    public class HomeController : Controller
    {
        LOGOKAMPEntities1 db = new LOGOKAMPEntities1();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ZTbLUseR formCollection)
        {
            using (db)
            {
                var model = db.ZTbLUseR.Where(x => x.Adi == formCollection.Adi && x.Sifresi == formCollection.Sifresi).SingleOrDefault();
                if (model.Yetkisi==1)
                {
                    return RedirectToAction("PlasiyerIndex", "Plasiyer");
                }
                else
                {
                    return RedirectToAction("MusteriIndex", "Musteri");
                }
            }
            
        }

      
    }
}