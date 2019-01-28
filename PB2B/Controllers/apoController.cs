using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB2B.Controllers
{
    public class apoController : Controller
    {
        // GET: apo
        public ActionResult Index()
        {
            return View();
        }

        // GET: apo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: apo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: apo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: apo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: apo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: apo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: apo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
