using System.Collections.Generic;
using System.Threading.Tasks;
using GimnasioJena.Abstracciones.Modelos.Progreso;

namespace GimnasioJena.Abstracciones.AccesoADatos.Progreso
{
    public interface IEntrenamientoAD
    {
        Task<List<EntrenamientoDto>> ObtenerTodosAD();

        Task<EntrenamientoDto> ObtenerPorDiaAD(string diaSemana);

        Task CrearAD(CrearEntrenamientoDto dto);
    }
}
