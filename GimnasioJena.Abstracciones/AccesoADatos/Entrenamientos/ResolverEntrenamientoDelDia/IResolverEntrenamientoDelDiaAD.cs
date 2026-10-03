using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ResolverEntrenamientoDelDia
{
    public interface IResolverEntrenamientoDelDiaAD
    {
        PlantillaDiaDto ResolverEntrenamientoDelDia(
            DateTime fechaSesion,
            byte diaSemana,
            int? idPlantillaDiaForzada
        );
    }
}
