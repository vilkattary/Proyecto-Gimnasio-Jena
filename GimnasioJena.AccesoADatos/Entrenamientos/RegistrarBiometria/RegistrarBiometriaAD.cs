using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.RegistrarBiometria;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;
using System;

namespace GimnasioJena.AccesoADatos.Entrenamientos.RegistrarBiometria
{
    public class RegistrarBiometriaAD : IRegistrarBiometriaAD
    {
        public int RegistrarBiometria(int userId, DateTime fecha, decimal pesoKg,
            decimal? grasaPorcentaje, decimal? musculoPorcentaje)
        {
            using (Contexto contexto = new Contexto())
            {
                var medicion = new UserBiometricsEntidad
                {
                    UserId = userId,
                    MeasurementDate = fecha.Date,
                    WeightKg = pesoKg,
                    BodyFatPercentage = grasaPorcentaje,
                    MuscleMassPercentage = musculoPorcentaje
                };

                contexto.UserBiometrics.Add(medicion);
                contexto.SaveChanges();

                return medicion.Id;
            }
        }
    }
}
