using GimnasioJena.Abstracciones.AccesoADatos.Home.ObtenerSeccionesHome;
using GimnasioJena.Abstracciones.Entidades.Home;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome;
using GimnasioJena.AccesoADatos.Home.ObtenerSeccionesHome;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Home.ObtenerSeccionesHome
{
    public class ObtenerSeccionesHomeLN : IObtenerSeccionesHomeLN
    {
        private readonly IObtenerSeccionesHomeAD _repositorio;

        public ObtenerSeccionesHomeLN()
        {
            _repositorio = new ObtenerSeccionesHomeAD();
        }

        public async Task<List<SeccionesHome>> ObtenerSeccionesHome()
        {
            return await _repositorio.ObtenerSeccionesHome();
        }
    }
}
