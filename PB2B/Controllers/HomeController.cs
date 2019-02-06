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
                var model = db.ZTbLUseR.Where(x=> x.Adi == formCollection.Adi && x.Sifresi == formCollection.Sifresi).SingleOrDefault();
                if (model==null)
                {
                    return View();
                }
                HttpCookie giris = new HttpCookie("login");
                giris["giris"] = "1";
                giris["userid"] = model.UserId.ToString();
                giris["clientref"] = model.CLIENTREF;
                giris["CariSTR"] = model.CariSTR;
                giris["adi"] = model.Adi;
                giris["yetki"] = model.Yetkisi.ToString();
                Session["userTip"] = (model.Yetkisi == 1) ? "Plasiyer" : "";
                Session["adi"] = model.Adi;
                Session["giris"] = "1";
                Session["SipID"] = model.SipID;
                Session["SipSTR"] = model.SipStr;
                Session["yetki"] = model.Yetkisi.ToString();
                Session["SATICI"] = model.CariSTR;
                giris.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(giris);
                if (model.Yetkisi==1)
                {
                   
                    return RedirectToAction("PlasiyerRaporlari", "Plasiyer",new { i=0});
                }
                else if(model.Yetkisi==3)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if(model.Yetkisi==2)
                {
                    return RedirectToAction("PlasiyerRaporlari", "Plasiyer", new { i = 0 });
                }
                else
                {
                    return View();
                }
            }
            
        }

      
    }
}