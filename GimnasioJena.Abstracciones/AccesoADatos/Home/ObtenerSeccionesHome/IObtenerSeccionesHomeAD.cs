using GimnasioJena.Abstracciones.Modelos.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Home.ObtenerSeccionesHome
{
    public interface IObtenerContenidoWebAD
    {
        Task<IEnumerable<ContenidoWebDto>> ObtenerPorPaginaAsync(string pagina);
    }
}
