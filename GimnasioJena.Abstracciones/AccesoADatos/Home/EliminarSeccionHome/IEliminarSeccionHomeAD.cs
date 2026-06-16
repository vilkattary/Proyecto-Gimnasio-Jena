using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Home.EliminarSeccionHome
{
    public interface IEliminarSeccionHomeAD
    {
        Task<bool> EliminarSeccionHome(int id);
    }
}
