using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ResolverEntrenamientoDelDia
{
    public interface IResolverEntrenamientoDelDiaLN
    {
        PlantillaDiaDto ResolverEntrenamientoDelDia(
            DateTime fechaSesion,
            int? idPlantillaDiaForzada
        );
    }
}
