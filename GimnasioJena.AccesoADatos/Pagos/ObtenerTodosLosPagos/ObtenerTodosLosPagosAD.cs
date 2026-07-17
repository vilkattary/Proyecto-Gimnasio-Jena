using GimnasioJena.Abstracciones.AccesoADatos.Pagos.ObtenerTodosLosPagos;
using GimnasioJena.Abstracciones.Modelos.Pagos;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Pagos.ObtenerTodosLosPagos
{
    public class ObtenerTodosLosPagosAD : IObtenerTodosLosPagosAD
    {
        private readonly Contexto _contexto;

        public ObtenerTodosLosPagosAD()
        {
            _contexto = new Contexto();
        }

        public List<PagoListadoDto> ObtenerTodosLosPagos()
        {
            var pagos =
                (from pago in _contexto.Pagos
                 join membresia in _contexto.Membresias
                    on pago.idMembresiaCliente equals membresia.idMembresiaCliente
                 join usuario in _contexto.Usuarios
                    on membresia.idUsuario equals usuario.idUsuario
                 join plan in _contexto.PlanesMembresia
                    on membresia.idPlanMembresia equals plan.idPlanMembresia
                 join metodo in _contexto.MetodoPagos
                    on pago.idMetodoPago equals metodo.idMetodoPago
                 join estado in _contexto.EstadoPagos
                    on pago.idEstadoPago equals estado.idEstadoPago
                 select new PagoListadoDto
                 {
                     idPago = pago.idPago,
                     idMembresiaCliente = pago.idMembresiaCliente,
                     nombreCliente = usuario.nombre + " " + usuario.apellido1 + " " + usuario.apellido2,
                     nombrePlan = plan.nombrePlan,
                     metodoPago = metodo.nombreMetodo,
                     estadoPago = estado.nombreEstado,
                     monto = pago.monto,
                     fechaPago = pago.fechaPago,
                     referenciaPago = pago.referenciaPago
                 })
                .OrderByDescending(p => p.fechaPago)
                .ToList();

            return pagos;
        }
    }
}