using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PB2B.Entity;
namespace PB2B.Controllers.Master
{
    public class MasterMusteriController : Controller
    {
        LOGOKAMPEntities1 db = new LOGOKAMPEntities1();
        // GET: MasterMusteri
        public ActionResult Sepet()
        {
            string CLIENTREF = Session["CLIENTREF"].ToString();
            var model = db.Z_SipLine.Where(x => x.CLIENTREF == CLIENTREF).ToList();
            return View(model);
        }
    }
}