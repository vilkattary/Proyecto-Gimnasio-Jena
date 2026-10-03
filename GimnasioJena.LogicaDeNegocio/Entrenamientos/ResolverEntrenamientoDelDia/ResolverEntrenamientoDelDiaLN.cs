using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ResolverEntrenamientoDelDia;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenamientos.ResolverEntrenamientoDelDia;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using GimnasioJena.AccesoADatos.Entrenamientos.ResolverEntrenamientoDelDia;
using System;

namespace GimnasioJena.LogicaDeNegocio.Entrenamientos.ResolverEntrenamientoDelDia
{
    public class ResolverEntrenamientoDelDiaLN : IResolverEntrenamientoDelDiaLN
    {
        private readonly IResolverEntrenamientoDelDiaAD _resolverEntrenamientoDelDiaAD;

        public ResolverEntrenamientoDelDiaLN()
        {
            _resolverEntrenamientoDelDiaAD = new ResolverEntrenamientoDelDiaAD();
        }

        public PlantillaDiaDto ResolverEntrenamientoDelDia(
            DateTime fechaSesion,
            int? idPlantillaDiaForzada
        )
        {
            // DiaSemana: 1=Lunes ... 7=Domingo (coherente con la convención del sistema).
            int diaDotNet = (int)fechaSesion.DayOfWeek; // 0=Domingo..6=Sábado
            byte diaSemana = (byte)(diaDotNet == 0 ? 7 : diaDotNet);

            return _resolverEntrenamientoDelDiaAD.ResolverEntrenamientoDelDia(
                fechaSesion,
                diaSemana,
                idPlantillaDiaForzada
            );
        }
    }
}
