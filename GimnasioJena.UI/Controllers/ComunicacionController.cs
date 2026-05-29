using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class ComunicacionController : Controller
    {
        // GET: Comunicacion
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comunicacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comunicacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comunicacion/Create
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

        // GET: Comunicacion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comunicacion/Edit/5
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

        // GET: Comunicacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comunicacion/Delete/5
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
