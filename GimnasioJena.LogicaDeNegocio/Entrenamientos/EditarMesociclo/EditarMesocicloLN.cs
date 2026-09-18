using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.EditarMesociclo;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.EditarMesociclo;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.EditarMesociclo;
using System;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.EditarMesociclo
{
    public class EditarMesocicloLN : IEditarMesocicloLN
    {
        private readonly IEditarMesocicloAD _editarMesocicloAD;

        public EditarMesocicloLN()
        {
            _editarMesocicloAD = new EditarMesocicloAD();
        }

        public bool EditarMesociclo(MesocicloDto modelo)
        {
            if (modelo == null || modelo.idMesociclo <= 0)
            {
                throw new ArgumentException("Debe indicar el mesociclo a editar.");
            }

            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                throw new ArgumentException("Debe indicar el nombre del mesociclo.");
            }

            modelo.Nombre = modelo.Nombre.Trim();
            modelo.FechaInicio = modelo.FechaInicio.Date;

            // Regla: el mesociclo dura 4 semanas (28 días).
            modelo.FechaFin = modelo.FechaInicio.AddDays(28);

            return _editarMesocicloAD.EditarMesociclo(modelo);
        }
    }
}
