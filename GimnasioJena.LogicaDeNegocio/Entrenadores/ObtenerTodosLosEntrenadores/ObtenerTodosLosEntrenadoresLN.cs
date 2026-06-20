using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerTodosLosEntrenadores;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Entrenadores.ObtenerTodosLosEntrenadores;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using GimnasioJena.AccesoADatos.Entrenadores.ObtenerTodosLosEntrenadores;
using System.Collections.Generic;

namespace GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerTodosLosEntrenadores
{
    public class ObtenerTodosLosEntrenadoresLN : IObtenerTodosLosEntrenadoresLN
    {
        private readonly IObtenerTodosLosEntrenadoresAD _obtenerTodosLosEntrenadoresAD;

        public ObtenerTodosLosEntrenadoresLN()
        {
            _obtenerTodosLosEntrenadoresAD = new ObtenerTodosLosEntrenadoresAD();
        }

        public List<EntrenadorListadoDto> ObtenerTodosLosEntrenadores()
        {
            return _obtenerTodosLosEntrenadoresAD.ObtenerTodosLosEntrenadores();
        }
    }
}