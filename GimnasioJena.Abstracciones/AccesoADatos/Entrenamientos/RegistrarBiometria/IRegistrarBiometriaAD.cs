using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.RegistrarBiometria
{
    public interface IRegistrarBiometriaAD
    {
        // Inserta una medición de biometría y devuelve su identificador.
        int RegistrarBiometria(int userId, DateTime fecha, decimal pesoKg,
            decimal? grasaPorcentaje, decimal? musculoPorcentaje);
    }
}
