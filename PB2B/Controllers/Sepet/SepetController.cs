using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB2B.classes;
using PB2B.Entity;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Entity.Migrations;

namespace PB2B.Controllers.Sepet
{
    [LoginAuthorize]
    [UserAuthorize]
    public class SepetController : Controller
    {
        LOGOKAMPEntities1 db = new LOGOKAMPEntities1();
        LKSDBEntities1 lksDb = new LKSDBEntities1();
        IDbConnection logoDdb = new SqlConnection(ConfigurationManager.ConnectionStrings["LKSDBDAPPER"].ConnectionString);
        public ActionResult Index()
        {
            string clientref = Session["CLIENTREF"].ToString();
            var model = db.Z_SipLine.Where(x => x.CLIENTREF == clientref && x.Durum == 0).OrderBy(x => x.STOCKNAME).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string clientref = Session["CLIENTREF"].ToString();
            string userRef = Session["userid"].ToString();
            var model = db.Z_SipLine.Where(x => x.CLIENTREF == clientref && x.Durum == 0).OrderBy(x => x.STOCKNAME).ToList();
            var seqModel = lksDb.LG_006_04_ORFICHESEQ.Take(1).SingleOrDefault();
            seqModel.LASTLREF++;
            int ORDREF = seqModel.LASTLREF;
            List<string> LUrunKodu = new List<string>();
            int Status = 0;
            int salesmanRef = 0;
            var cariModel = lksDb.C_INT_CARIONEKRAN_006.Where(x => x.LOGREF.ToString() == clientref).SingleOrDefault();
            if (cariModel != null)
            {
                string SLSMAN = cariModel.SATICI;
                var slsmanModel = lksDb.LG_SLSMAN.Where(x => x.CODE == SLSMAN && x.FIRMNR.ToString() == baglanti.GFirma).SingleOrDefault();
                if (slsmanModel == null)
                {
                    salesmanRef = 0;
                }
                else
                {
                    salesmanRef = slsmanModel.LOGICALREF;
                }
            }


            using (logoDdb)
            {
                int i = 0;

                foreach (var item in model)
                {

                    if (i == 0)
                    {

                        int result = logoDdb.Execute("INSERT INTO LG_" + baglanti.GFirma + "_" + baglanti.GDonem + "_ORFICHE (LOGICALREF,TRCODE,FICHENO,DATE_,TIME_,DOCODE,SPECODE,CYPHCODE,CLIENTREF,RECVREF,ACCOUNTREF,CENTERREF,SOURCEINDEX,SOURCECOSTGRP,UPDCURR,ADDDISCOUNTS,TOTALDISCOUNTS,TOTALDISCOUNTED,ADDEXPENSES,TOTALEXPENSES,TOTALPROMOTIONS,TOTALVAT,GROSSTOTAL,NETTOTAL,REPORTRATE,REPORTNET,GENEXP1,GENEXP2,GENEXP3,GENEXP4,EXTENREF,PAYDEFREF,PRINTCNT,BRANCH,DEPARTMENT,STATUS,CAPIBLOCK_CREATEDBY,CAPIBLOCK_CREADEDDATE,CAPIBLOCK_CREATEDHOUR,CAPIBLOCK_CREATEDMIN,CAPIBLOCK_CREATEDSEC,CAPIBLOCK_MODIFIEDBY,CAPIBLOCK_MODIFIEDDATE,CAPIBLOCK_MODIFIEDHOUR,CAPIBLOCK_MODIFIEDMIN,CAPIBLOCK_MODIFIEDSEC,SALESMANREF,SHPTYPCOD,SHPAGNCOD,GENEXCTYP,LINEEXCTYP,TRADINGGRP,TEXTINC,SITEID,RECSTATUS,ORGLOGICREF,FACTORYNR,WFSTATUS,SHIPINFOREF,CUSTORDNO,SENDCNT,DLVCLIENT,DOCTRACKINGNR,CANCELLED,ORGLOGOID,OFFERREF,OFFALTREF,TYP,ALTNR,ADVANCEPAYM,TRCURR,TRRATE,TRNET,PAYMENTTYPE,ONLYONEPAYLINE,OPSTAT,WITHPAYTRANS,PROJECTREF,WFLOWCRDREF,UPDTRCURR,AFFECTCOLLATRL,POFFERBEGDT,POFFERENDDT,REVISNR,LASTREVISION,CHECKAMOUNT,SLSOPPRREF,SLSACTREF,SLSCUSTREF,AFFECTRISK,TOTALADDTAX,TOTALEXADDTAX) " +
                    " Values ('" + ORDREF + "',1,'" + item.FICHENO + "', getdate(), 268440134, '', 'INTERNET', '', " + item.CLIENTREF + ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0,0.0, 0.0, 0.0, 0.0, 0, 0, 0, '" + "', '', '', '', 0, 3, 0, 0, 0, " + Status + ", 1, '" + DateTime.Now.ToString("MM/dd/yy") + "', " + DateTime.Now.Hour + ", " + DateTime.Now.Minute + ", " + DateTime.Now.Second + ", 0, NULL, 0, 0, 0, " + salesmanRef + ", '', '', 1, 0, '', 0, 0, 1, 0, 0, 0, 0, '', 0, 0, '', 0, '', 0, 0, 0, 0, 0.0, 0, 0.0,0.0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, '', 0, 0, 0, 0, 0, 0, 0.0, 0.0)");
                        if (result == 0) { break; }
                        logoDdb.Execute("update LG_" + baglanti.GFirma + "_" + baglanti.GDonem + "_ORFICHESEQ set  LASTLREF=" + ORDREF + " where ID=1");
                    }
                    var seqLineModel = lksDb.LG_006_04_ORFLINESEQ.Take(1).SingleOrDefault();
                    var kdvmik = (Convert.ToDouble(item.SATIRTUTARI) * 0.18).ToString().Replace(",", ".");
                    var kdvTutar = (Convert.ToDouble(item.SATIRTUTARI) * 1.18).ToString().Replace(",", ".");
                    int UOMREF = 23;
                    int USREF = 5;
                    var PrCLisTReF = "0";
                    seqLineModel.LASTLREF++;

                    int lineResult = logoDdb.Execute("INSERT INTO LG_" + baglanti.GFirma + "_" + baglanti.GDonem + "_ORFLINE (LOGICALREF,STOCKREF,ORDFICHEREF,CLIENTREF,LINETYPE,PREVLINEREF,PREVLINENO,DETLINE,LINENO_,TRCODE,DATE_,TIME_,GLOBTRANS,CALCTYPE,CENTERREF,ACCOUNTREF,VATACCREF,VATCENTERREF,PRACCREF,PRCENTERREF,PRVATACCREF,PRVATCENREF,PROMREF,SPECODE,DELVRYCODE,AMOUNT,PRICE,TOTAL,SHIPPEDAMOUNT,DISCPER,DISTCOST,DISTDISC,DISTEXP,DISTPROM,VAT,VATAMNT,VATMATRAH,LINEEXP,UOMREF,USREF,UINFO1,UINFO2,UINFO3,UINFO4,UINFO5,UINFO6,UINFO7,UINFO8,VATINC,CLOSED,DORESERVE,INUSE,DUEDATE,PRCURR,PRPRICE,REPORTRATE,BILLEDITEM,PAYDEFREF,EXTENREF,CPSTFLAG,SOURCEINDEX,SOURCECOSTGRP,BRANCH,DEPARTMENT,LINENET,SALESMANREF,STATUS,DREF,TRGFLAG,SITEID,RECSTATUS,ORGLOGICREF,FACTORYNR,WFSTATUS,NETDISCFLAG,NETDISCPERC,NETDISCAMNT,CONDITIONREF,DISTRESERVED,ONVEHICLE,CAMPAIGNREFS1,CAMPAIGNREFS2,CAMPAIGNREFS3,CAMPAIGNREFS4,CAMPAIGNREFS5,"
 + "POINTCAMPREF,CAMPPOINT,PROMCLASITEMREF,REASONFORNOTSHP,CMPGLINEREF,PRRATE,GROSSUINFO1,GROSSUINFO2,CANCELLED,DEMPEGGEDAMNT,TEXTINC,OFFERREF,ORDERPARAM,ITEMASGREF,EXIMAMOUNT,OFFTRANSREF,ORDEREDAMOUNT,ORGLOGOID,TRCURR,TRRATE,WITHPAYTRANS,PROJECTREF,POINTCAMPREFS1,POINTCAMPREFS2,POINTCAMPREFS3,POINTCAMPREFS4,CAMPPOINTS1,CAMPPOINTS2,CAMPPOINTS3,CAMPPOINTS4,CMPGLINEREFS1,CMPGLINEREFS2,CMPGLINEREFS3,CMPGLINEREFS4,PRCLISTREF,AFFECTCOLLATRL,FCTYP,PURCHOFFNR,DEMFICHEREF,DEMTRANSREF,ALTPROMFLAG,VARIANTREF,REFLVATACCREF,REFLVATOTHACCREF,PRIORITY,AFFECTRISK,BOMREF,BOMREVREF,ROUTINGREF,OPERATIONREF,WSREF,ADDTAXRATE,ADDTAXCONVFACT,ADDTAXAMOUNT,ADDTAXACCREF,ADDTAXCENTERREF,ADDTAXAMNTISUPD,ADDTAXDISCAMOUNT,EXADDTAXRATE,EXADDTAXCONVF,EXADDTAXAMNT,EUVATSTATUS,ADDTAXVATMATRAH) Values " +
  "('" + seqLineModel.LASTLREF + "'," + item.STOCKREF + ", " + ORDREF + ", " + item.CLIENTREF + ", 0, 0, 0, 0, " + i + ", 1, getdate(), 268440134, 0,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', " + item.AMOUNT.ToString().Replace(",", ".") + ", " + item.PRICE.ToString().Replace(",", ".") + ", " + item.SATIRTUTARI.ToString().Replace(",", ".") + ", 0.0, 0.0, 0.0,0.0, 0.0, 0.0,18, " + kdvmik + ", " + kdvTutar + ", '', " + UOMREF + ", " + USREF + ", 1.0,1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0, 0, 1, 0, '" + DateTime.Now.ToString("MM/dd/yy") + "', 160, " + item.PRICE.ToString().Replace(",", ".") + ", 1, 0, 0, 0, 0, 0, 0, 0, 0, " + item.SATIRTUTARI.ToString().Replace(",", ".") + "," + salesmanRef + " , " + Status + ", 0, 0, 0, 1, 0, 0, 0, 0, 0.0,0.0, 0, 0.0, 0.0, 0, 0, 0, 0, 0, 0, 0.0, 0, 0, 0, 0.0, 0.0,0.0, 0, 0.0, 0, 0, 0, 0, 0.0, 0, 0.0, '', 0, 0.0, 0, 0, 0, 0, 0, 0, 0.0,0.0, 0.0, 0.0, 0, 0, 0, 0, '" + PrCLisTReF + "', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.0, 0.0,0.0, 0, 0, 0, 0.0, 0.0, 0.0, 0.0, 0, 0.0)");
                    LUrunKodu.Add(item.STOCKNAME + "-" + item.AMOUNT);
                    i++;
                    logoDdb.Execute("update LG_" + baglanti.GFirma + "_" + baglanti.GDonem + "_ORFLINESEQ set LASTLREF=" + seqLineModel.LASTLREF + " where ID=1");
                    item.Durum = 2;
                    db.Z_SipLine.AddOrUpdate(item);
                }


              
                var userModel = db.ZTbLUseR.Where(x => x.UserId.ToString() == userRef).SingleOrDefault();
                userModel.SipID++;
                db.ZTbLUseR.AddOrUpdate(userModel);
                db.SaveChanges();
                Session["SipID"] = userModel.SipID;

                //mail at
                string Firma_Bilgisi = Session["Firma_Bilgisi"].ToString();
                List<string> kime = new List<string>();
                //   var cariModel=lksDb.C_INT_CARIONEKRAN_006.Where(x => x.LOGREF.ToString() == clientref).SingleOrDefault();
                kime.Add("alpay@mentalsoft.net");
                PMail.MailGonderSiparis("Sayın " + Firma_Bilgisi, "", kime, LUrunKodu, "MENAB2B Sipariş FeedBack");
            }
            model = new List<Z_SipLine>();
            return View(model);
        }
        [HttpPost]
        public ActionResult SepetMail()
        {


            return RedirectToAction("Sepet");
        }

        public ActionResult SatirSil(string id)
        {
            var model = db.Z_SipLine.Where(x => x.LOGICALREF.ToString() == id).SingleOrDefault();
            db.Z_SipLine.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}