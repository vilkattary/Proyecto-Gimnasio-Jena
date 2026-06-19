using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerTodosLosEntrenadores;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Entrenadores.ObtenerTodosLosEntrenadores
{
    public class ObtenerTodosLosEntrenadoresAD : IObtenerTodosLosEntrenadoresAD
    {
        public List<EntrenadorListadoDto> ObtenerTodosLosEntrenadores()
        {
            using (var contexto = new Contexto())
            {
                return contexto.Entrenadores
                    .Include(e => e.Usuario)
                    .OrderBy(e => e.Usuario.nombre)
                    .Select(e => new EntrenadorListadoDto
                    {
                        idEntrenador = e.idEntrenador,
                        idUsuario = e.idUsuario,
                        nombreCompleto = e.Usuario.nombre + " " + e.Usuario.apellido1 + " " + e.Usuario.apellido2,
                        correo = e.Usuario.correo,
                        especialidad = e.especialidad,
                        fechaContratacion = e.fechaContratacion,
                        estado = e.estado
                    })
                    .ToList();
            }
        }
    }
}