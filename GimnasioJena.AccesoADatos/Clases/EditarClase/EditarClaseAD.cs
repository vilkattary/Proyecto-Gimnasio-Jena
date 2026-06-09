using GimnasioJena.Abstracciones.AccesoADatos.Clases.EditarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Clases.EditarClase
{
    public class EditarClaseAD : IEditarClaseAD
    {
        private Contexto _elContexto;

        public EditarClaseAD()
        {
            _elContexto = new Contexto();
        }

        public int EditarClases(ClaseEditarDto claseAEditar)
        {
            ClaseEntidad claseEnBaseDeDatos = _elContexto.Clases
                .FirstOrDefault(c => c.idClaseProgramada == claseAEditar.idClaseProgramada);

            if (claseEnBaseDeDatos == null)
            {
                return 0;
            }

            claseEnBaseDeDatos.idTipoClase = claseAEditar.idTipoClase;
            claseEnBaseDeDatos.idUsuarioEntrenador = claseAEditar.idUsuarioEntrenador;
            claseEnBaseDeDatos.idEstadoClase = claseAEditar.idEstadoClase;
            claseEnBaseDeDatos.fechaClase = claseAEditar.fechaClase;
            claseEnBaseDeDatos.horaInicio = claseAEditar.horaInicio;
            claseEnBaseDeDatos.horaFin = claseAEditar.horaFin;
            claseEnBaseDeDatos.cupoMaximo = claseAEditar.cupoMaximo;
            claseEnBaseDeDatos.ubicacion = claseAEditar.ubicacion;
            claseEnBaseDeDatos.observaciones = claseAEditar.observaciones;
            claseEnBaseDeDatos.fechaModificacion = claseAEditar.fechaModificacion;

            return _elContexto.SaveChanges();
        }
    }
}