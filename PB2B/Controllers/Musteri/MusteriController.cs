using System;
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
namespace PB2B.Controllers.Musteri
{
    
    public class MusteriController : Controller
    {
        LKSDBEntities1 lDb = new LKSDBEntities1();
        IDbConnection dDbLKSDB = new SqlConnection();
        // GET: Musteri
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CariExtre()
        {
            List<CARIEXTRE> list = new List<CARIEXTRE>();
            using (dDbLKSDB)
            {
                string CLIENTREF = Session["CLIENTREF"].ToString();
                list = dDbLKSDB.Query<CARIEXTRE>("SELECT *, (SELECT SUM(b.TTUTAR)AS BAKIYE FROM ISRG_Hesap_Extresi_" + baglanti.GFirma + " AS b Where b.ISLEMTARIHI<=K.ISLEMTARIHI AND CLIENTREF=" + CLIENTREF + ")AS BAKIYE  FROM(select LOGICALREF,CLIENTREF,ISLEMTARIHI,ISLEMTURU,TRANNO,SOURCEFREF, (CASE WHEN BORC_ALACAK=0 THEN TUTAR ELSE 0 END) AS ALACAK,(CASE WHEN BORC_ALACAK=1 THEN TUTAR ELSE 0 END) AS BORC,VADE,LEFT(ISLEMACIKLAMASI,20) AS ISLEMACIKLAMASI from ISRG_Hesap_Extresi_" + baglanti.GFirma + " Where CLIENTREF=" + CLIENTREF + " ) AS K ORDER BY ISLEMTARIHI,TRANNO").ToList();
            }
            return View(list);
        }

        public ActionResult Faturalarim()
        {
            string CLIENTREF = Session["CLIENTREF"].ToString();
            return View(lDb.ISRG_Faturalar_006.Where(x=>x.CariRef.ToString()==CLIENTREF).OrderBy(x=>x.DATE_).ToList());
        }

        public ActionResult FaturaDetay(string id)
        {
            return View(lDb.ISRG_FaturaDetaY_006.Where(x => x.INVOICEREF.ToString() == id).ToList());
        }
    }
}