using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult MiPerfil()
        {
            return View();
        }
        public ActionResult MiMembresia()
        {
            return View();
        }
        public ActionResult ReservarClases()
        {
            return View();
        }

        public ActionResult MisReservas()
        {
            return View();
        }

    }
}
