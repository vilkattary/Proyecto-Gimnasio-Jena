using GimnasioJena.Abstracciones.AccesoADatos.Clases.EditarClase;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.EditarClase;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Clases.EditarClase;
using GimnasioJena.LogicaDeNegocio.Clases.ObtenerClasePorId;
using GimnasioJena.LogicaDeNegocio.General.Fechas;
using System;

namespace GimnasioJena.LogicaDeNegocio.Clases.EditarClase
{
    public class EditarClaseLN : IEditarClaseLN
    {
        private IEditarClaseAD _editarClaseAD;
        private IFechasLN _fechasLN;
        private IObtenerClasePorIdLN _obtenerClasePorIdLN;

        public EditarClaseLN()
        {
            _editarClaseAD = new EditarClaseAD();
            _fechasLN = new FechasLN();
            _obtenerClasePorIdLN = new ObtenerClasePorIdLN();
        }

        public bool EditarClase(ClaseEditarDto claseAEditar)
        {
            try
            {
                ClaseListadoDto datosAntes = _obtenerClasePorIdLN.ObtenerClasePorId(claseAEditar.idClaseProgramada);
                claseAEditar.fechaModificacion = _fechasLN.ObtenerFechaActual();
                int cantidadDeDatosAlmacenados = _editarClaseAD.EditarClases(claseAEditar);
                return cantidadDeDatosAlmacenados > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
