using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.RegistrarBiometria;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.RegistrarBiometria;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.RegistrarBiometria;
using System;
using System.Globalization;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.RegistrarBiometria
{
    public class RegistrarBiometriaLN : IRegistrarBiometriaLN
    {
        private readonly IRegistrarBiometriaAD _registrarBiometriaAD;

        public RegistrarBiometriaLN()
        {
            _registrarBiometriaAD = new RegistrarBiometriaAD();
        }

        public ResultadoBiometriaDto RegistrarBiometria(RegistrarBiometriaDto modelo)
        {
            if (modelo == null)
            {
                return Fallo("No se recibieron datos de la medición.");
            }

            if (modelo.UserId <= 0)
            {
                return Fallo("Usuario no válido.");
            }

            if (modelo.WeightKg <= 0m || modelo.WeightKg > 500m)
            {
                return Fallo("El peso debe estar entre 0 y 500 kg.");
            }

            if (modelo.BodyFatPercentage.HasValue &&
                (modelo.BodyFatPercentage.Value < 0m || modelo.BodyFatPercentage.Value > 100m))
            {
                return Fallo("El porcentaje de grasa debe estar entre 0 y 100.");
            }

            if (modelo.MuscleMassPercentage.HasValue &&
                (modelo.MuscleMassPercentage.Value < 0m || modelo.MuscleMassPercentage.Value > 100m))
            {
                return Fallo("El porcentaje de músculo debe estar entre 0 y 100.");
            }

            // Fecha: por defecto hoy; no se permiten fechas futuras.
            DateTime fecha = DateTime.Today;
            if (!string.IsNullOrWhiteSpace(modelo.MeasurementDate) &&
                DateTime.TryParse(modelo.MeasurementDate, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime parseada))
            {
                fecha = parseada.Date;
            }

            if (fecha > DateTime.Today)
            {
                return Fallo("La fecha de la medición no puede ser futura.");
            }

            int id = _registrarBiometriaAD.RegistrarBiometria(
                modelo.UserId,
                fecha,
                modelo.WeightKg,
                modelo.BodyFatPercentage,
                modelo.MuscleMassPercentage);

            return new ResultadoBiometriaDto
            {
                Exito = true,
                Mensaje = "Medición registrada correctamente.",
                idBiometria = id
            };
        }

        private static ResultadoBiometriaDto Fallo(string mensaje)
        {
            return new ResultadoBiometriaDto
            {
                Exito = false,
                Mensaje = mensaje
            };
        }
    }
}
