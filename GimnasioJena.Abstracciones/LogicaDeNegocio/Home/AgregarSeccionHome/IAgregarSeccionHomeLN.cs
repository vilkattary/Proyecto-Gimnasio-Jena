using GimnasioJena.Abstracciones.Modelos.Home;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Home.AgregarSeccionHome
{
    public interface IAgregarSeccionHomeLN
    {
        Task<bool> AgregarSeccionHome(AgregarSeccionHomeDto dto);
    }
}
