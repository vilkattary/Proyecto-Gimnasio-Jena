using System;

namespace GimnasioJena.Abstracciones.AccesoADatos.Clases.CambiarEstadoClase
{
    public interface ICambiarEstadoClaseAD
    {
        bool CambiarEstadoClase(int idClaseProgramada, DateTime fechaModificacion);
    }
}
