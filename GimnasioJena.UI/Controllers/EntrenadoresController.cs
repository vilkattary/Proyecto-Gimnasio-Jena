using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ENTRENADOR")]
    public class EntrenadoresController : Controller
    {
        private readonly IObtenerUsuarioPorIdLN _obtenerUsuarioServicio;

        public EntrenadoresController(IObtenerUsuarioPorIdLN obtenerUsuarioServicio)
        {
            _obtenerUsuarioServicio = obtenerUsuarioServicio;
        }
        // GET: Entrenador
        public ActionResult Index()
        {
            var identityUserId = User.Identity.GetUserId();
            var perfil = await _obtenerUsuarioServicio.ObtenerEntrenadorPorId(identityUserId);
            return View(perfil);
        }

        // GET: Entrenador/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Entrenador/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Entrenador/Create
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

        // GET: Entrenador/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Entrenador/Edit/5
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

        // GET: Entrenador/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Entrenador/Delete/5
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
