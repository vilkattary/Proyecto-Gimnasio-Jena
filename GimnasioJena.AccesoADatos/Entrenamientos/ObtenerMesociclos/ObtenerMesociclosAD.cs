using GimnasioJena.Abstracciones.AccesoADatos.Entrenamientos.ObtenerMesociclos;
using GimnasioJena.Abstracciones.Modelos.Entrenamientos;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenamientos.ObtenerMesociclos
{
    public class ObtenerMesociclosAD : IObtenerMesociclosAD
    {
        public List<MesocicloListadoDto> ObtenerMesociclos()
        {
            using (Contexto contexto = new Contexto())
            {
                return contexto.Mesociclos
                    .OrderByDescending(m => m.FechaInicio)
                    .Select(m => new MesocicloListadoDto
                    {
                        idMesociclo = m.idMesociclo,
                        Nombre = m.Nombre,
                        FechaInicio = m.FechaInicio,
                        FechaFin = m.FechaFin,
                        EsActivo = m.EsActivo,
                        CantidadDias = m.PlantillasDia.Count
                    })
                    .ToList();
            }
        }
    }
}
