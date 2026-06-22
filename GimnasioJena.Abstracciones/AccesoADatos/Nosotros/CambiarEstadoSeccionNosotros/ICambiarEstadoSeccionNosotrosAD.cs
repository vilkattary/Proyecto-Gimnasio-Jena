using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Nosotros.CambiarEstadoSeccionNosotros
{
    public interface ICambiarEstadoSeccionNosotrosAD
    {
        Task<bool> ToggleEstadoAsync(int id);
    }
}
