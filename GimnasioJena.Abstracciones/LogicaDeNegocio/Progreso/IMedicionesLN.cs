using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso
{
    public interface IMedicionesLN
    {
        Task<RegistroMedicionMultipleDto> GenerarFormularioLN(string identityUserId);

        Task GuardarMedicionesLN(RegistroMedicionMultipleDto dto);
    }
}
