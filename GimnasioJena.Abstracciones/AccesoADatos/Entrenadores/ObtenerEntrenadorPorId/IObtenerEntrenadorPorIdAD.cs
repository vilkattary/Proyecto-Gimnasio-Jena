using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId
{
    public interface IObtenerEntrenadorPorIdAD
    {
        Task<EntrenadorDto> ObtenerEntrenadorPorId(string identityUserId);
        EntrenadorDto ObtenerEntrenadorPorId(int idEntrenador);
    }
}
