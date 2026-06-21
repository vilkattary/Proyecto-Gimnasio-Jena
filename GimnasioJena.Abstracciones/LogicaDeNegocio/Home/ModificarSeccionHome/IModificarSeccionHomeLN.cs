using GimnasioJena.Abstracciones.Modelos.Home;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome
{
    public interface IModificarContenidoWebLN
    {
        Task<bool> EjecutarAsync(ModificarContenidoWebDto dto);
    }
}
