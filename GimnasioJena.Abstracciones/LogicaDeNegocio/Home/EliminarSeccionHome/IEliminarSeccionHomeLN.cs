using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Home.EliminarSeccionHome
{
    public interface IEliminarSeccionHomeLN
    {
        Task<bool> EliminarSeccionHome(int id);
    }
}
