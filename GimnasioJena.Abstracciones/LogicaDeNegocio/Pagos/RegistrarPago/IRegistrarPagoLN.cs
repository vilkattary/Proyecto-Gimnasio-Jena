using GimnasioJena.Abstracciones.Modelos.Pagos;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Pagos.RegistrarPago
{
    public interface IRegistrarPagoLN
    {
        int RegistrarPago(PagoCrearDto pago);
    }
}