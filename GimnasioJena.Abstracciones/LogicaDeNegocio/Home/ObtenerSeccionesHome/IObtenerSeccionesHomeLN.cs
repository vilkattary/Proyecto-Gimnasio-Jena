using GimnasioJena.Abstracciones.Entidades.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome
{
    public interface IObtenerSeccionesHomeLN
    {
        Task<List<SeccionesHome>> ObtenerSeccionesHome();
    }
}
