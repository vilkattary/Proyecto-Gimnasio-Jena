using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso
{
    public interface IEvolucionAvanzadaLN
    {
        Task GuardarOClonarRutinaLN(GuardarRutinaManualDto dto, bool esClonacion, bool puedeGestionarRutinas);

        Task<GuardarRutinaManualDto> ObtenerRutinaParaClonarLN(int idEntrenamiento);

        Task<System.Collections.Generic.List<EntrenamientoDto>> ObtenerPlantillasDisponiblesLN();

        Task RegistrarProgresoClienteLN(RegistroProgresoAvanzadoDto dto, string identityUserId, bool esCliente);

        Task<MetricasEvolucionAvanzadaDto> CompilarDashboardAvanzadoLN(int idUsuario, string periodo);

        Task GuardarEntrenamientoClaseLN(CrearEntrenamientoClaseDto dto);

        Task<RegistrarProgresoClaseDto> ObtenerHojaProgresoParaClienteLN(int idClaseProgramada, int idUsuario);

        Task GuardarProgresoClienteLN(RegistrarProgresoClaseDto dto);
    }
}
