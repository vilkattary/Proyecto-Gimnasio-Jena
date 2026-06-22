using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Home.CambiarEstadoSeccionHome
{
    public interface ICambiarEstadoSeccionHomeLN
    {
        Task<bool> EjecutarAsync(int id);
    }
}
