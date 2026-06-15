using GimnasioJena.Abstracciones.Modelos.Home;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Home.ModificarSeccionHome
{
    public interface IModificarSeccionHomeAD
    {
        Task<bool> ModificarSeccionHome(ModificarSeccionHomeDto dto);
    }
}
