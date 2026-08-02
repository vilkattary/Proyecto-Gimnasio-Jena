using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.Modelos.Membresias
{
    public class MiMembresiaViewModel
    {
        public MembresiaClienteDto membresiaActual { get; set; }

        public List<PlanMembresiaListadoDto> planesDisponibles { get; set; }

        public MiMembresiaViewModel()
        {
            planesDisponibles =
                new List<PlanMembresiaListadoDto>();
        }
    }
}
