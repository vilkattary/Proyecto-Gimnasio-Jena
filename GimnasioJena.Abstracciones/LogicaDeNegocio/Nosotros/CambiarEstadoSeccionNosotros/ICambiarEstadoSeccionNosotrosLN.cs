using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.CambiarEstadoSeccionNosotros
{
    public interface ICambiarEstadoSeccionNosotrosLN
    {
        Task<bool> EjecutarAsync(int id);
    }
}
