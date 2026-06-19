using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerTodosLosEntrenadores
{
    public interface IObtenerTodosLosEntrenadoresLN
    {
        List<EntrenadorListadoDto> ObtenerTodosLosEntrenadores();
    }
}