using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.ObtenerHorarioSemanalPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.ObtenerHorarioSemanalPorId;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.HorariosSemanales
    .ObtenerHorarioSemanalPorId;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales
    .ObtenerHorarioSemanalPorId
{
    public class ObtenerHorarioSemanalPorIdLN
        : IObtenerHorarioSemanalPorIdLN
    {
        private readonly IObtenerHorarioSemanalPorIdAD
            _obtenerHorarioSemanalPorIdAD;

        public ObtenerHorarioSemanalPorIdLN()
        {
            _obtenerHorarioSemanalPorIdAD =
                new ObtenerHorarioSemanalPorIdAD();
        }

        public HorarioSemanalEditarDto
            ObtenerHorarioSemanalPorId(int idHorario)
        {
            return _obtenerHorarioSemanalPorIdAD
                .ObtenerHorarioSemanalPorId(idHorario);
        }
    }
}