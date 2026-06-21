using GimnasioJena.Abstracciones.Modelos.Home;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Nosotros.AgregarSeccionNosotros
{
    public interface IAgregarSeccionNosotrosLN
    {
        Task<bool> AgregarSeccionNosotros(AgregarSeccionHomeDto dto);
    }
}
