using GimnasioJena.Abstracciones.Modelos.Asistencias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.RegistrarAsistencia
{
    public interface IRegistrarAsistenciaLN
    {
        ResultadoAsistenciaDto RegistrarAsistencia(
            AsistenciaCrearDto asistencia,
            int idClaseProgramada
        );
    }
}