using GimnasioJena.Abstracciones.Modelos.Pagos;
using System;

namespace GimnasioJena.Abstracciones.AccesoADatos.Pagos.RegistrarPago
{
    public interface IRegistrarPagoAD
    {
        PagoMembresiaValidacionDto ObtenerDatosMembresia(
            int idMembresiaCliente
        );

        int RegistrarPagoYActualizarMembresia(
            PagoCrearDto pago,
            DateTime? nuevaFechaInicio,
            DateTime? nuevaFechaFin,
            int? nuevasClasesDisponibles,
            int? nuevoEstadoMembresia
        );

        bool ExisteMetodoPago(int idMetodoPago);

        bool ExisteEstadoPago(int idEstadoPago);
    }
}