using GimnasioJena.Abstracciones.Modelos.Membresias;

namespace GimnasioJena.Abstracciones.AccesoADatos.Membresias.ObtenerMembresiaPorId
{
    public interface IObtenerMembresiaPorIdAD
    {
        MembresiaEditarDto ObtenerMembresiaPorId(int idMembresiaCliente);
        MembresiaClienteDto ObtenerDetalleMembresiaPorId(int idMembresiaCliente);
    }
}