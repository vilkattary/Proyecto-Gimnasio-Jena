using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Entrenadores.ObtenerEntrenadorPorId
{
    public class ObtenerEntrenadorPorIdLN
    {
        private readonly IObtenerEntrenadorPorIdAD _repositorio;

        public ObtenerEntrenadorPorIdLN(IObtenerEntrenadorPorIdAD repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<EntrenadorPerfilDto> ObtenerEntrenadorPorId(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                return null;

            return await _repositorio.ObtenerEntrenadorPorId(identityUserId);
        }
    }
}
