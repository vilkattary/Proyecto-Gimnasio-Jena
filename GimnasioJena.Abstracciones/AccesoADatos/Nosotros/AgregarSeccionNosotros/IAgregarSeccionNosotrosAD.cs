using GimnasioJena.Abstracciones.Modelos.Home;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Nosotros.AgregarSeccionNosotros
{
    public interface IAgregarSeccionNosotrosAD
    {
        Task<bool> AgregarSeccionNosotros(AgregarSeccionHomeDto dto);
    }
}
