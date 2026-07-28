using GimnasioJena.Abstracciones.AccesoADatos.Membresias.EditarMembresia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Membresias.EditarMembresia;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.AccesoADatos.Membresias.EditarMembresia;
using System;

namespace GimnasioJena.LogicaDeNegocio.Membresias.EditarMembresia
{
    public class EditarMembresiaLN : IEditarMembresiaLN
    {
        private readonly IEditarMembresiaAD _editarMembresiaAD;

        public EditarMembresiaLN()
        {
            _editarMembresiaAD = new EditarMembresiaAD();
        }

        public bool EditarMembresia(MembresiaEditarDto membresia)
        {
            if (membresia == null)
                return false;

            if (membresia.idMembresiaCliente <= 0)
                return false;

            if (membresia.idEstadoMembresia <= 0)
                return false;

            if (membresia.fechaInicio == default(DateTime))
                return false;

            return _editarMembresiaAD
                .EditarMembresia(membresia) > 0;
        }
    }
}