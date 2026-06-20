using GimnasioJena.Abstracciones.AccesoADatos.Asistencias.RegistrarAsistencia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Asistencias.RegistrarAsistencia;
using GimnasioJena.Abstracciones.Modelos.Asistencias;
using GimnasioJena.AccesoADatos.Asistencias.RegistrarAsistencia;

namespace GimnasioJena.LogicaDeNegocio.Asistencias.RegistrarAsistencia
{
    public class RegistrarAsistenciaLN : IRegistrarAsistenciaLN
    {
        private readonly IRegistrarAsistenciaAD _registrarAsistenciaAD;

        public RegistrarAsistenciaLN()
        {
            _registrarAsistenciaAD = new RegistrarAsistenciaAD();
        }

        public bool RegistrarAsistencia(AsistenciaCrearDto asistencia)
        {
            if (asistencia == null)
                return false;

            if (asistencia.idReserva <= 0)
                return false;

            return _registrarAsistenciaAD.RegistrarAsistencia(asistencia) > 0;
        }
    }
}