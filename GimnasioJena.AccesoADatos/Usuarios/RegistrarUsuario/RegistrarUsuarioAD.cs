using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using GimnasioJena.AccesoADatos.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioAD : IRegistrarUsuarioAD
    {
        private readonly Contexto _contexto;

        public RegistrarUsuarioAD(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<UsuarioDto> RegistrarUsuario(UsuarioCrearDto dto)
        {
            var entidad = new UsuarioEntidad
            {
                identityUserId = dto.identityUserId,
                nombre = dto.nombre,
                apellido1 = dto.apellido1,
                apellido2 = dto.apellido2,
                identificacion = dto.identificacion,
                correo = dto.correo,
                telefono = dto.telefono,
                fechaRegistro = DateTime.Now,
                estado = true
            };

            _contexto.Usuarios.Add(entidad);
            await _contexto.SaveChangesAsync();

            return new UsuarioDto
            {
                idUsuario = entidad.idUsuario,
                identityUserId = entidad.identityUserId,
                nombre = entidad.nombre,
                apellido1 = entidad.apellido1,
                apellido2 = entidad.apellido2,
                identificacion = entidad.identificacion,
                correo = entidad.correo,
                telefono = entidad.telefono,
                fechaRegistro = entidad.fechaRegistro,
                estado = entidad.estado
            };
        }
    }
}
