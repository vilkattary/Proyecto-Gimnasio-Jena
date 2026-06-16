using GimnasioJena.Abstracciones.Modelos.Home;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Home.AgregarSeccionHome
{
    public interface IAgregarSeccionHomeAD
    {
        Task<bool> AgregarSeccionHome(AgregarSeccionHomeDto dto);
    }
}
