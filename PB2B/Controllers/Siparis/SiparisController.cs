﻿using PB2B.classes;
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
        public ActionResult SiparisVer(string searchParam,string stokOlan)
        {
            var model= lDb.A_MNTL_STOK_2018.AsQueryable();
            if (stokOlan=="on")
            {
                model = model.Where(x => x.BAKIYE > 0);
            }
            return View(model.Where(x => (x.URUNADI + x.URUNKODU + x.MARKA + x.MODEL).Replace(" ", "").Contains(searchParam.Replace(" ", ""))).ToList());
        }

        public PartialViewResult SiparisUrunGec(string itemref)
        {
            var model = lDb.A_MNTL_STOK_2018.Where(x => x.LOGICALREF.ToString() == itemref).SingleOrDefault();
            double CARIISK = Convert.ToDouble(Session["CARIISK"].ToString());
            URUNGEC urungec = new URUNGEC();
            if (model != null)
            {
                urungec = new URUNGEC()
                {
                    ITEMREF = model.LOGICALREF,
                    NAME = model.URUNADI,
                    CODE = model.URUNKODU,
                    ListeFiyati = model.ListeFiyati.ToString("N"),
                    OnerilenFiyat =Convert.ToDecimal(Convert.ToDouble(model.ListeFiyati)*(1-(CARIISK/100))).ToString("N"),
                    ORAN = model.ORAN.ToString(),
                    INDORAN = ((1 - model.INDORAN) * 100).ToString(),
                    ALTORAN= ((1 - model.ALTORAN) * 100).ToString(),
                    TabanFiyat = model.Maliyet
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
                STOCKREF =_urungec.ITEMREF.ToString(),
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