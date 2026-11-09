using GimnasioJena.Abstracciones.Modelos.Pagos;
using System;

namespace GimnasioJena.Abstracciones.AccesoADatos.Pagos.RegistrarPago
{
    public interface IRegistrarPagoAD
    {
        PagoMembresiaValidacionDto ObtenerDatosMembresia(
            int idMembresiaCliente
        );

        PagoMembresiaValidacionDto ObtenerDatosPlanActivo(
            int idPlanMembresia
        );

        int RegistrarPagoYActualizarMembresia(
            PagoCrearDto pago,
            DateTime? nuevaFechaInicio,
            DateTime? nuevaFechaFin,
            int? nuevasClasesDisponibles,
            int? nuevoEstadoMembresia
        );

        int RegistrarPrimerPagoManual(
            PagoCrearDto pago,
            DateTime fechaInicio,
            DateTime fechaFin,
            int? clasesDisponibles
        );

        bool ExisteMetodoPago(int idMetodoPago);

        bool ExisteEstadoPago(int idEstadoPago);

        bool ExisteReferenciaPago(string referenciaPago);
    }
}