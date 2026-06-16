using GimnasioJena.Abstracciones.Modelos.Home;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Home.ModificarSeccionHome
{
    public interface IModificarSeccionHomeLN
    {
        Task<bool> ModificarSeccionHome(ModificarSeccionHomeDto dto);
    }
}
