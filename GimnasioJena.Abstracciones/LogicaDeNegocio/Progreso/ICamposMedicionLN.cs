using System.Collections.Generic;
using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso
{
    public interface ICamposMedicionLN
    {
        Task<List<CampoMedicionDto>> ObtenerTodosLN();

        Task CrearLN(CampoMedicionDto dto);

        Task CambiarEstadoLN(int idCampo);
    }
}
