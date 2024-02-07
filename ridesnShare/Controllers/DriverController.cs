using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ridesnShare.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        public ActionResult Index()
        {
            return View();
        }

        // GET: Driver/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Driver/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Driver/Create
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

        // GET: Driver/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Driver/Edit/5
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

        // GET: Driver/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Driver/Delete/5
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
