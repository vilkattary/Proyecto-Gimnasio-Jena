using GimnasioJena.Abstracciones.AccesoADatos.Reservas.EditarReserva;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Reservas.EditarReserva;
using GimnasioJena.AccesoADatos.Reservas.EditarReserva;

namespace GimnasioJena.LogicaDeNegocio.Reservas.EditarReserva
{
    public class EditarReservaLN : IEditarReservaLN
    {
        private readonly IEditarReservaAD _editarReservaAD;

        public EditarReservaLN()
        {
            _editarReservaAD = new EditarReservaAD();
        }

        public bool CambiarEstadoReserva(int idReserva, int idEstadoReserva)
        {
            return _editarReservaAD.CambiarEstadoReserva(idReserva, idEstadoReserva);
        }
    }
}