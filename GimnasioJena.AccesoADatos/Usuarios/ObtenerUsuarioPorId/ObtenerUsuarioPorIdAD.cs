using GimnasioJena.Abstracciones.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using GimnasioJena.Abstracciones.Modelos.Membresias;
using GimnasioJena.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Usuarios.ObtenerUsuarioPorId
{
    public class ObtenerUsuarioPorIdAD : IObtenerUsuarioPorIdAD
    {
        private readonly Contexto _contexto;

        public ObtenerUsuarioPorIdAD(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<UsuarioPerfilDto> ObtenerUsuarioPorId(string identityUserId)
        {
            var usuario = await _contexto.Usuarios
                .FirstOrDefaultAsync(u => u.identityUserId == identityUserId);

            if (usuario == null) return null;

            var membresia = await _contexto.Membresias
                .Include(m => m.EstadoMembresia)
                .Include(m => m.PlanMembresia)
                .FirstOrDefaultAsync(m => m.idUsuario == usuario.idUsuario);

            MembresiaClienteDto membresiaDto = null;

            if (membresia != null)
            {
                membresiaDto = new MembresiaClienteDto
                {
                    idMembresiaCliente = membresia.idMembresiaCliente,
                    idUsuario = membresia.idUsuario,
                    nombreCliente = usuario.nombre + " " + usuario.apellido1,
                    nombrePlan = membresia.PlanMembresia.nombrePlan,
                    estadoMembresia = membresia.EstadoMembresia.nombreEstado,
                    fechaInicio = membresia.fechaInicio,
                    fechaFin = membresia.fechaFin,
                    clasesDisponibles = membresia.clasesDisponibles,
                    observaciones = membresia.observaciones,
                    precio = membresia.PlanMembresia.precio
                };
            }

            return new UsuarioPerfilDto
            {
                idUsuario = usuario.idUsuario,
                nombre = usuario.nombre,
                apellido1 = usuario.apellido1,
                apellido2 = usuario.apellido2,
                identificacion = usuario.identificacion,
                correo = usuario.correo,
                telefono = usuario.telefono,
                direccion = usuario.direccion,
                fotoPerfil = usuario.fotoPerfil,
                estado = usuario.estado,
                Membresia = membresiaDto
            };
        }
    }
}
