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
      
        public ActionResult PlasiyerIndex(string direktAc)
        {
            List<C_INT_CARIONEKRAN_006> _list = new List<C_INT_CARIONEKRAN_006>();
            var model = tDb.C_INT_CARIONEKRAN_006.AsQueryable();
            if (Session["SATICI"].ToString() != "HEPSİ")
            {
                string clientref = Session["SATICI"].ToString();
                model = model.Where(x => x.SATICI == clientref);
            }
            if (direktAc=="1")
            {
                _list= model.OrderBy(x => x.Firma_Bilgisi).ToList();
            }
            return View(_list);
        }

        [HttpPost]
        public ActionResult PlasiyerIndex(string searchParam, string isSepet, string isBakiyeli)
        {
            var model = tDb.C_INT_CARIONEKRAN_006.AsQueryable();
            if (Session["SATICI"].ToString() != "HEPSİ")
            {
                string clientref = Session["SATICI"].ToString();
                model = model.Where(x => x.SATICI ==clientref );
            }
            if (isSepet == "on")
            {

            }
            if (isBakiyeli == "on")
            {
                model = model.Where(x => x.Bakiye > 10);
            }
            if (searchParam != "")
            {
                model = model.Where(x => (x.Firma_Bilgisi + x.KODU).Replace(" ", "").Contains(searchParam.Replace("", "")));
            }
            return View(model.OrderBy(x => x.Firma_Bilgisi).ToList());
        }

        public ActionResult PlasiyerYonlendir(int id)
        {
            var model = tDb.C_INT_CARIONEKRAN_006.Where(x => x.LOGREF == id).SingleOrDefault();
            if (model != null)
            {
                Session["Firma_Bilgisi"] = model.Firma_Bilgisi;
                Session["CODE"] = model.KODU;
                Session["CLIENTREF"] = model.LOGREF;
                return RedirectToAction("Index", "Musteri");
            }
            return RedirectToAction("Index");
        }

        public ActionResult PlasiyerRaporlari()
        {
            ViewData["Plasiyer"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult PlasiyerRaporlari(string plasiyer)
        {

            ViewData["Plasiyer"] = plasiyer;
            return View();
        }

        public ActionResult PlasiyerRaporPartial(string Plasiyer)
        {

            var query = tDb.JSrG_GenelCari_006_04.AsQueryable();
            if (Session["yetki"].ToString() == "1")
            {
                string SATICI= Session["SATICI"].ToString();
                query = query.Where(x => x.Plasiyer ==SATICI);
            }else if (!String.IsNullOrEmpty(Plasiyer) && Plasiyer!="Hepsi")
            {
                query = tDb.JSrG_GenelCari_006_04.Where(x => x.Plasiyer == Plasiyer);
            }
          
            var model = new JSrG_GenelCari_006_04();
            model.Bakiye = query.Sum(x => x.Bakiye);
            model.ACIKRISK = query.Sum(x => x.ACIKRISK);
            model.CIKISLAR = query.Sum(x => x.CIKISLAR);
            model.FATURASAYISI = query.Sum(x => x.FATURASAYISI);
            model.GIRISLER = query.Sum(x => x.GIRISLER);
            model.ONCEKIBAKIYE = query.Sum(x => x.ONCEKIBAKIYE);
            model.SONFATURATARIHI = query.Max(x => x.SONFATURATARIHI);
            model.SONTAHSILATTARIHI = query.Max(x => x.SONTAHSILATTARIHI);
            model.YILLIKSATISTUTARI = query.Sum(x => x.YILLIKSATISTUTARI);
            model.Plasiyer = Plasiyer;
            model.LOGICALREF = query.Count();
            Session["SATICI"] = Plasiyer;
            
            return View(model);
        }

        public ActionResult PlasiyerAramaAc(string CariSTR)
        {
            Session["SATICI"] = CariSTR;
            return RedirectToAction("PlasiyerIndex", "Plasiyer", new { i = 0 });
        }

        public PartialViewResult EnCokSatilanUrun(string Plasiyer)
        {
            if (String.IsNullOrEmpty(Plasiyer))
            {
                var model = tDb.J_SRG_SATICISATILAN_006_04.Take(10).ToList();
                return PartialView(model);
            }
            else
            {
                var model = tDb.J_SRG_SATICISATILAN_006_04.Where(x => x.CSATICI == Plasiyer).OrderByDescending(x => x.NETTUTAR).Take(10).ToList();
                return PartialView(model);
            }

        }

        public PartialViewResult EnCokSatilanFirma(string Plasiyer)
        {
            if (String.IsNullOrEmpty(Plasiyer))
            {
                var model = tDb.J_SRG_SATICIFIRMA_006_04.Take(10).ToList();
                return PartialView(model);
            }
            else
            {
                var model = tDb.J_SRG_SATICIFIRMA_006_04.Where(x => x.CSATICI == Plasiyer).OrderByDescending(x => x.NETTUTAR).Take(10).ToList();
                return PartialView(model);
            }

        }

        public ActionResult CekSenetLine()
        {
            string Plasiyer = Session["SATICI"].ToString();
            var query = tDb.JSRG_CEK_SENET_DURUM.AsQueryable();
            if (Plasiyer!="Hepsi")
            {
                query = query.Where(x => x.Plasiyer == Plasiyer);
            }

            
            return View(query.OrderBy(x=>x.TARIH).ToList());
        }

        public ActionResult Faturalar()
        {

            string Plasiyer = Session["SATICI"].ToString();
            var query = tDb.ISRG_Faturalar_006.AsQueryable();
            if (Plasiyer != "Hepsi")
            {
                query = query.Where(x => x.SPECODE == Plasiyer);
            }

            DateTime dateTime = DateTime.Now.AddDays(-31);
            return View(query.Where(x=>x.DATE_>=dateTime).OrderBy(x => x.DATE_).ToList());
            
        }
        public ActionResult FaturaDetay(string id)
        {
            var model = new List<ISRG_FaturaDetaY_006>();
            var invoiceModel = tDb.ISRG_Faturalar_006.Where(x => x.FICHENO == id).SingleOrDefault();
            TempData["faturaModel"] = invoiceModel;
            if (invoiceModel != null)
            {
                model = tDb.ISRG_FaturaDetaY_006.Where(x => x.INVOICEREF == invoiceModel.LOGICALREF).ToList();
            }
            return View(model);
        }


    }
}