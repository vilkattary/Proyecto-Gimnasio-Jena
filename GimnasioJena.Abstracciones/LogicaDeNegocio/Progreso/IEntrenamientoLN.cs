using System.Collections.Generic;
using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso
{
    public interface IEntrenamientoLN
    {
        Task<List<EntrenamientoDto>> ObtenerTodosLN();

        Task<EntrenamientoDto> ObtenerEntrenamientoDeHoyLN();

        Task CrearEntrenamientoLN(CrearEntrenamientoDto dto);
    }
}
