using GimnasioJena.Abstracciones.Modelos.Asistencias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.RegistrarAsistencia
{
    public interface IRegistrarAsistenciaLN
    {
        bool RegistrarAsistencia(AsistenciaCrearDto asistencia);
    }
}