using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.RegistrarMesociclo;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.RegistrarMesociclo;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.RegistrarMesociclo;
using System;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.RegistrarMesociclo
{
    public class RegistrarMesocicloLN : IRegistrarMesocicloLN
    {
        private readonly IRegistrarMesocicloAD _registrarMesocicloAD;

        public RegistrarMesocicloLN()
        {
            _registrarMesocicloAD = new RegistrarMesocicloAD();
        }

        public int RegistrarMesociclo(MesocicloDto modelo)
        {
            if (modelo == null)
            {
                throw new ArgumentException("Debe indicar los datos del mesociclo.");
            }

            if (string.IsNullOrWhiteSpace(modelo.Nombre))
            {
                throw new ArgumentException("Debe indicar el nombre del mesociclo.");
            }

            modelo.Nombre = modelo.Nombre.Trim();
            modelo.FechaInicio = modelo.FechaInicio.Date;

            // Regla: el mesociclo dura 4 semanas (28 días).
            modelo.FechaFin = modelo.FechaInicio.AddDays(28);

            return _registrarMesocicloAD.RegistrarMesociclo(modelo);
        }
    }
}
