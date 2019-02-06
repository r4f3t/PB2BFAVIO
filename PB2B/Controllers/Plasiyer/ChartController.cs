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
namespace PB2B.Controllers.Plasiyer
{
    public partial class ChartController : Controller
    {
        IDbConnection dDb = new SqlConnection(ConfigurationManager.ConnectionStrings["LKSDBDAPPER"].ConnectionString);
        // GET: Chart
        public ActionResult SatisChart()
        {

            return View();
        }
        public PartialViewResult SatisChartPartial()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult GetSatisJson()
        {
            var model = dDb.Query<CHARTSATIS>("select * from (select Sum(Satis) as Satis,Zaman,Yil from ZZ_ChartSatisPL group by Zaman,Yil  ) as R order by Yil,Zaman ").ToList();
            return Json(model,JsonRequestBehavior.AllowGet);
        }
    }
}