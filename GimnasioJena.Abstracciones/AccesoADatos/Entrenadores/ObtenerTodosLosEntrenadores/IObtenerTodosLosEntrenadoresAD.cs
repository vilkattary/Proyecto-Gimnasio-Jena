using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using System.Collections.Generic;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerTodosLosEntrenadores
{
    public interface IObtenerTodosLosEntrenadoresAD
    {
        List<EntrenadorListadoDto> ObtenerTodosLosEntrenadores();
    }
}