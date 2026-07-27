using GimnasioJena.Abstracciones.AccesoADatos.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.Abstracciones.General.DiasSemana;
using GimnasioJena.Abstracciones.LogicaDeNegocio.HorariosSemanales.ObtenerHorariosSemanales;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.HorariosSemanales.ObtenerHorariosSemanales;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales.ObtenerHorariosSemanales
{
    public class ObtenerHorariosSemanalesLN : IObtenerHorariosSemanalesLN
    {
        private readonly IObtenerHorariosSemanalesAD
            _obtenerHorariosSemanalesAD;

        public ObtenerHorariosSemanalesLN()
        {
            _obtenerHorariosSemanalesAD =
                new ObtenerHorariosSemanalesAD();
        }

        public List<HorarioSemanalListadoDto>
            ObtenerHorariosSemanales()
        {
            var horarios =
                _obtenerHorariosSemanalesAD
                    .ObtenerHorariosSemanales();

            foreach (var horario in horarios)
            {
                horario.nombreDia =
                    DiasSemana.ObtenerNombre(
                        horario.diaSemana
                    );
            }

            return horarios
                .OrderBy(h => h.diaSemana)
                .ThenBy(h => h.horaInicio)
                .ToList();
        }
    }
}
