using PB2B.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB2B.Entity;
using PB2B.Models;

namespace PB2B.Controllers.Siparis
{
    [LoginAuthorize]
    [UserAuthorize]
    public class SiparisController : Controller
    {
        LKSDBEntities1 lDb = new LKSDBEntities1();
        LOGOKAMPEntities1 db = new LOGOKAMPEntities1();
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

        public PartialViewResult SiparisUrunGec(string itemref)
        {
            var model = lDb.A_MNTL_STOK_2018.Where(x => x.LOGICALREF.ToString() == itemref).SingleOrDefault();
            URUNGEC urungec = new URUNGEC();
            if (model != null)
            {
                urungec = new URUNGEC()
                {
                    ITEMREF = model.LOGICALREF,
                    NAME = model.URUNADI,
                    CODE = model.URUNKODU,
                    ListeFiyati = model.ListeFiyati.ToString("N"),
                    OnerilenFiyat = model.OnerilenFiyat.ToString("N"),
                    ORAN = model.ORAN.ToString(),
                    INDORAN = ((1 - model.INDORAN) * 100).ToString(),
                    ALTORAN= ((1 - model.ALTORAN) * 100).ToString(),
                    TabanFiyat = model.TabanFiyat.ToString("N")
                };
            }
            return PartialView(urungec);
        }
        [HttpPost]
        public ActionResult SiparisUrunGec(URUNGEC _urungec,string itemref)
        {
            var model = new Z_SipLine() {
                CLIENTREF = Session["CLIENTREF"].ToString(),
                AMOUNT = Convert.ToDecimal(_urungec.MIKTAR),
                DATE_ = DateTime.Now,
                PRICE = Convert.ToDecimal(_urungec.SONFIYAT.Replace(".",",")),
                MIKINDORAN = Convert.ToDecimal(_urungec.INDORAN.Replace(".", ",")),
                SATIRTUTARI = Convert.ToDecimal(_urungec.GENELTOPLAM.Replace(".", ",")),
                STOCKREF = itemref,
                SIPID = Convert.ToInt32(Session["SipID"].ToString()),
                FICHENO = Session["SipSTR"].ToString() + "-" + Session["SipID"].ToString(),
                STOCKNAME = _urungec.NAME,
                STOCKCODE = _urungec.CODE,
                Durum = 0
            };
            db.Z_SipLine.Add(model);
            db.SaveChanges();
            return RedirectToAction("SiparisVer");
        }
    }
}