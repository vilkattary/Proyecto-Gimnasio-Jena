using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class MembresiasController : Controller
    {
        // GET: Membresias
        public ActionResult Index()
        {
            return View();
        }

        // GET: Membresias/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Membresias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Membresias/Create
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

        // GET: Membresias/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Membresias/Edit/5
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

        // GET: Membresias/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Membresias/Delete/5
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
