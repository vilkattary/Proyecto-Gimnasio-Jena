using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Nosotros.EliminarSeccionNosotros
{
    public interface IEliminarSeccionNosotrosAD
    {
        Task<bool> EliminarSeccionNosotros(int id);
    }
}
