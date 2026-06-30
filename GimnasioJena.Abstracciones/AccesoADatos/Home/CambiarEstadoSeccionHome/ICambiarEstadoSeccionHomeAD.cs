using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Home.CambiarEstadoSeccionHome
{
    public interface ICambiarEstadoSeccionHomeAD
    {
        Task<bool> ToggleEstadoAsync(int id);
    }
}
