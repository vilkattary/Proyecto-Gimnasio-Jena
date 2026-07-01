using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.ObtenerMembresiaPorId
{
    public interface IObtenerMembresiaPorIdLN
    {
        MembresiaEditarDto ObtenerMembresiaPorId(int idMembresiaCliente);
        MembresiaClienteDto ObtenerDetalleMembresiaPorId(int idMembresiaCliente);
    }
}