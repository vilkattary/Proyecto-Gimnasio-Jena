using GimnasioJena.Abstracciones.Modelos.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ObtenerSeccionesHome
{
    public interface IObtenerContenidoWebLN
    {
        Task<IEnumerable<ContenidoWebDto>> EjecutarAsync(string pagina);
        Task<IEnumerable<ContenidoWebDto>> EjecutarTodosAsync(string pagina);
    }
}
