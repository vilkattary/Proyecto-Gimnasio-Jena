using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.EliminarSeccionNosotros
{
    public interface IEliminarSeccionNosotrosLN
    {
        Task<bool> EliminarSeccionNosotros(int id);
    }
}
