using PB2B.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB2B.Entity;
namespace PB2B.Controllers.Siparis
{
    [LoginAuthorize]
    [UserAuthorize]
    public class SiparisController : Controller
    {
        LKSDBEntities1 lDb = new LKSDBEntities1();
        // GET: Siparis
    
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SiparisVer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SiparisVer(string searchParam)
        {
            var model = lDb.A_MNTL_STOK_2018.Where(x => (x.URUNADI + x.URUNKODU + x.MARKA + x.MODEL).Replace(" ", "").Contains(searchParam.Replace(" ", ""))).ToList();
            return View(model);
        }

        public PartialViewResult SiparisUrunGec()
        {

            return PartialView();
        }
    }
}