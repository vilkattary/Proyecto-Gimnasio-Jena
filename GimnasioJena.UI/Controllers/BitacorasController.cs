using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.LogicaDeNegocio.Bitacora;
using System.Web.Mvc;

namespace GimnasioJena.UI.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class BitacorasController : Controller
    {
        private readonly IObtenerBitacorasLN _obtenerBitacorasLN;

        public BitacorasController()
        {
            _obtenerBitacorasLN = new ObtenerBitacoraLN();
        }

        public ActionResult Index()
        {
            var bitacoras = _obtenerBitacorasLN.ObtenerBitacoras();
            return View(bitacoras);
        }
    }
}