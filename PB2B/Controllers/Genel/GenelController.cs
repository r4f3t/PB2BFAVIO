using PB2B.classes;
using PB2B.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB2B.Controllers.Genel
{
    [LoginAuthorize]
    public class GenelController : Controller
    {

        LKSDBEntities1 lDb = new LKSDBEntities1();
        // GET: Genel
        public ActionResult Stoklar()
        {
            string yetki = Session["yetki"].ToString();
            if (yetki == "1" || yetki == "2")
            {
                ViewBag.MasterGenel = "~/Views/Shared/LTEMASTER.cshtml";
            }
            else
            {
                ViewBag.MasterGenel = "~/Views/Shared/LTEAdminMASTER.cshtml";
            }

            return View();
        }

        public PartialViewResult StoklistPartial(string searchParams)
        {
            List<A_MNTL_STOK_2018> model = lDb.A_MNTL_STOK_2018.Where(x => (x.URUNADI + x.URUNKODU + x.MARKA).Replace(" ", "").
            Contains(searchParams.Replace(" ", ""))).ToList();
            return PartialView(model);
        }

        public ActionResult Cariler()
        {
            string yetki = Session["yetki"].ToString();
            if (yetki == "1" || yetki == "2")
            {
                ViewBag.MasterGenel = "~/Views/Shared/LTEMASTER.cshtml";
            }
            else
            {
                ViewBag.MasterGenel = "~/Views/Shared/LTEAdminMASTER.cshtml";
            }

            return View();
        }
        public PartialViewResult CarilistPartial(string searchParams, string hareket, string eski)
        {
            string SATICI = Session["SATICI"].ToString();
            var query = lDb.JSrG_GenelCari_006_04.AsQueryable();
            if (SATICI != "HEPSI")
            {
                query = lDb.JSrG_GenelCari_006_04.Where(x => x.Plasiyer == SATICI);
            }
            if (hareket != "Tümü")
            {
                query = query.Where(x => ((hareket == "Hareketli") ? x.CIKISLAR > 0 : x.CIKISLAR == 0));
            }
            if (eski != "Tümü")
            {
                DateTime _date = DateTime.Now.AddDays(-31);
                query = query.Where(x => ((eski == "Yeni") ? x.ADDDATE > _date : x.ADDDATE < _date));
            }
            if (searchParams != "")
            {
                query = query.Where(x => (x.FirmaAdı + x.FirmaKodu).Replace(" ", "").Contains(searchParams.Replace(" ", "")));
            }
            return PartialView(query.OrderBy(x => x.FirmaAdı).ToList());
        }

        public ActionResult Plasiyerler()
        {
            string yetki = Session["yetki"].ToString();
            if (yetki == "1" || yetki == "2")
            {
                ViewBag.MasterGenel = "~/Views/Shared/LTEMASTER.cshtml";
            }
            else
            {
                ViewBag.MasterGenel = "~/Views/Shared/LTEAdminMASTER.cshtml";
            }

            return View();
        }
        public PartialViewResult PlasiyerlistPartial()
        {
            string SATICI = Session["SATICI"].ToString();
            var query = lDb.XX_SRG_CLFGenel_006.AsQueryable();
            if (SATICI != "HEPSI" && SATICI!="")
            {
                query = lDb.XX_SRG_CLFGenel_006.Where(x => x.CariSatici == SATICI);
            }
          
            return PartialView(query.OrderBy(x => x.CariSatici).ToList());
        }

    }
}