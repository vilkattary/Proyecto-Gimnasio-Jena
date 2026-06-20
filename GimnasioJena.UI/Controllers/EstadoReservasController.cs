using GimnasioJena.Abstracciones.LogicaDeNegocio.EstadoReservas.ObtenerEstadoReservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    public class EstadoReservasController : Controller
    {
        private readonly IObtenerEstadoReservasLN _obtenerEstadoReservasLN;
     
        public EstadoReservasController(IObtenerEstadoReservasLN obtenerEstadoReservasLN /*, otras */)
        {
            _obtenerEstadoReservasLN = obtenerEstadoReservasLN;
        }

        public ActionResult InscritosClase(int idClase)
        {
            
            var inscritos =0 ;
            ViewBag.EstadosReserva = _obtenerEstadoReservasLN
                                         .ObtenerTodosLosEstadosReservas()
                                         .ToList();
            return View(inscritos);
        }

        [HttpPost]
        public ActionResult CambiarEstadoReserva(int idReserva, int idEstadoReserva)
        {
            
            return RedirectToAction("InscritosClase");
        }
    }
}