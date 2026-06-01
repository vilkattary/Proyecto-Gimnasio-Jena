using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioLN : IRegistrarUsuarioLN
    {
        private readonly IRegistrarUsuarioAD _repositorio;

        public RegistrarUsuarioLN(IRegistrarUsuarioAD repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<UsuarioDto> RegistrarUsuario(UsuarioCrearDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.nombre))
                throw new Exception("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.correo))
                throw new Exception("El correo es obligatorio.");

            return await _repositorio.RegistrarUsuario(dto);
        }
    }
}
