using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId
{
    public class ObtenerUsuarioPorIdLN : IObtenerUsuarioPorIdLN
    {
        private readonly IObtenerUsuarioPorIdAD _repositorio;

        public ObtenerUsuarioPorIdLN(IObtenerUsuarioPorIdAD repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<UsuarioPerfilDto> ObtenerUsuarioPorId(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                return null;

            return await _repositorio.ObtenerUsuarioPorId(identityUserId);
        }
    }
}
