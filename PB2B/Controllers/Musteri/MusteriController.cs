﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using PB2B.Models;
using PB2B.Entity;
using PB2B.classes;

namespace PB2B.Controllers.Musteri
{
    [LoginAuthorize]
    public class MusteriController : Controller
    {
        LKSDBEntities1 lDb = new LKSDBEntities1();
        IDbConnection dDbLKSDB = baglanti.DapperConnection();
        // GET: Musteri
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult MusteriRaporPArtial()
        {
            string clientRef = Session["CLIENTREF"].ToString();
            return View(lDb.JSrG_GenelCari_006_04.Where(x => x.LOGICALREF.ToString() == clientRef).SingleOrDefault());
        }
        public ActionResult CariExtre()
        {

            List<CARIEXTRE> list = new List<CARIEXTRE>();
            
            string CLIENTREF = Session["CLIENTREF"].ToString();
            decimal? sayi=lDb.ISRG_Hesap_Extresi_006_04.Where(x => x.CLIENTREF.ToString() == CLIENTREF && x.ISLEMTARIHI.Value.Year < 2019).Sum(x=>x.TTUTAR);
            list = dDbLKSDB.Query<CARIEXTRE>("SELECT *, (SELECT SUM(b.TTUTAR)AS BAKIYE FROM ISRG_Hesap_Extresi_" + baglanti.GFirma + "_" + baglanti.GDonem + " AS b Where b.rank<=K.rank AND CLIENTREF=" + CLIENTREF + ")AS BAKIYE  FROM(select rank,LOGICALREF,CLIENTREF,ISLEMTARIHI,ISLEMTURU,TRANNO,SOURCEFREF, (CASE WHEN BORC_ALACAK=0 THEN TUTAR ELSE 0 END) AS ALACAK,(CASE WHEN BORC_ALACAK=1 THEN TUTAR ELSE 0 END) AS BORC,VADE,LEFT(ISLEMACIKLAMASI,20) AS ISLEMACIKLAMASI from ISRG_Hesap_Extresi_" + baglanti.GFirma + "_" + baglanti.GDonem + " Where CLIENTREF=" + CLIENTREF + " ) AS K ORDER BY rank,TRANNO").ToList();
            return View(list);
        }

        public ActionResult Faturalarim()
        {
            string CLIENTREF = Session["CLIENTREF"].ToString();
            return View(lDb.ISRG_Faturalar_006.Where(x => x.CariRef.ToString() == CLIENTREF).OrderBy(x => x.DATE_).ToList());
        }

        public ActionResult FaturaDetay(string id)
        {
            var model = new List<ISRG_FaturaDetaY_006>();
            var invoiceModel = lDb.ISRG_Faturalar_006.Where(x => x.FICHENO == id).SingleOrDefault();
            TempData["faturaModel"] = invoiceModel;
            if (invoiceModel != null)
            {
                model = lDb.ISRG_FaturaDetaY_006.Where(x => x.INVOICEREF == invoiceModel.LOGICALREF).ToList();
            }
            return View(model);
        }

        public ActionResult Siparisler()
        {
            string clientref = Session["CLIENTREF"].ToString();
            var sipModel = lDb.I_SRG_EKSIKSIPARISLER_006_04.Where(x => x.CLIENTREF.ToString() == clientref).ToList();
            return View(sipModel);
        }

        public ActionResult CekSenetlerim()
        {
            string clientref = Session["CLIENTREF"].ToString();
            var query = lDb.JSRG_CEK_SENET_DURUM.AsQueryable();
            query = query.Where(x => x.CARDREF.ToString() == clientref);
            return View(query.OrderBy(x => x.TARIH).ToList());
        }

    }
}