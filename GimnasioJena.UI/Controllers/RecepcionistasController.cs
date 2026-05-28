using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class RecepcionistasController : Controller
    {
        // GET: Recepcionistas
        public ActionResult Index()
        {
            return View();
        }

        // GET: Recepcionistas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Recepcionistas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recepcionistas/Create
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

        // GET: Recepcionistas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Recepcionistas/Edit/5
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

        // GET: Recepcionistas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Recepcionistas/Delete/5
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
