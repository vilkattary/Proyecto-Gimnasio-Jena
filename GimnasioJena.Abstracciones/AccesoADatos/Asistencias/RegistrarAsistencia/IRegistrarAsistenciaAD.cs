using GimnasioJena.Abstracciones.Modelos.Asistencias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Asistencias.RegistrarAsistencia
{
    public interface IRegistrarAsistenciaAD
    {
        int RegistrarAsistencia(AsistenciaCrearDto asistencia);
    }
}