using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId
{
    public interface IObtenerEntrenadorPorIdLN
    {
        Task<EntrenadorDto> ObtenerEntrenadorPorId(string identityUserId);
    }
}
