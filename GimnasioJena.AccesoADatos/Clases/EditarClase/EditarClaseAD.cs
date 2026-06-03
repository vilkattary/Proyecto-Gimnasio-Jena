using GimnasioJena.Abstracciones.AccesoADatos.Clases.EditarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using System;
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

        public int EditarClases(ClaseEditarDto ClaseAEditar)
        {
            ClaseEntidad claseEnBaseDeDatos = _elContexto.Clases
                .Where(c => c.idClaseProgramada == ClaseAEditar.idClaseProgramada)
                .FirstOrDefault();

            if (claseEnBaseDeDatos != null)
            {
                claseEnBaseDeDatos.nombreClase = ClaseAEditar.nombreClase;
                claseEnBaseDeDatos.tipoClase = ClaseAEditar.tipoClase;
                claseEnBaseDeDatos.nombreEntrenador = ClaseAEditar.nombreEntrenador;
                claseEnBaseDeDatos.estadoClase = ClaseAEditar.estadoClase;
                claseEnBaseDeDatos.fechaClase = ClaseAEditar.fechaClase;
                claseEnBaseDeDatos.horaInicio = ClaseAEditar.horaInicio;
                claseEnBaseDeDatos.horaFin = ClaseAEditar.horaFin;
                claseEnBaseDeDatos.cupoMaximo = ClaseAEditar.cupoMaximo;
                claseEnBaseDeDatos.ubicacion = ClaseAEditar.ubicacion;
                claseEnBaseDeDatos.fechaModificacion = DateTime.Now;

                return _elContexto.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
    }
}
