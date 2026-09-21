using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso
{
    public interface IProgresoLN
    {
        Task<ResumenEvolucionDto> GenerarResumenEvolucionLN(int idUsuario, string filtroTiempo);

        Task GuardarProgresoDiarioLN(ProgresoDiaDto dto);

        Task<int> ObtenerIdUsuarioActualLN(string identityUserId);
    }
}
