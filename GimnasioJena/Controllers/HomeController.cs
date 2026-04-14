using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Planes()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Conoce más sobre Gimnasio Jena, nuestra misión y nuestros valores.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Ponte en contacto con nosotros. Estamos aquí para ayudarte.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(FormCollection form)
        {
            TempData["MensajeExito"] = "¡Gracias por tu mensaje! Nos pondremos en contacto contigo pronto.";
            return RedirectToAction("Contact");
        }
    }
}