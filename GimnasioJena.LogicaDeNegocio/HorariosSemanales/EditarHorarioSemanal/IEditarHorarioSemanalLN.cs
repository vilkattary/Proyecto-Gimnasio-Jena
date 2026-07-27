using GimnasioJena.Abstracciones.AccesoADatos
    .HorariosSemanales.EditarHorarioSemanal;
using GimnasioJena.Abstracciones.LogicaDeNegocio
    .HorariosSemanales.EditarHorarioSemanal;
using GimnasioJena.Abstracciones.Modelos.HorariosSemanales;
using GimnasioJena.AccesoADatos.HorariosSemanales
    .EditarHorarioSemanal;

namespace GimnasioJena.LogicaDeNegocio.HorariosSemanales
    .EditarHorarioSemanal
{
    public class EditarHorarioSemanalLN
        : IEditarHorarioSemanalLN
    {
        private readonly IEditarHorarioSemanalAD
            _editarHorarioSemanalAD;

        public EditarHorarioSemanalLN()
        {
            _editarHorarioSemanalAD =
                new EditarHorarioSemanalAD();
        }

        public void EditarHorarioSemanal(
            HorarioSemanalEditarDto modelo
        )
        {
            _editarHorarioSemanalAD
                .EditarHorarioSemanal(modelo);
        }
    }
}