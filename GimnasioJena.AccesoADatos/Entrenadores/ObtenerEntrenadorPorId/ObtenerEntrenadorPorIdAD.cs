using GimnasioJena.Abstracciones.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId;
using GimnasioJena.Abstracciones.Modelos.Entrenadores;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entrenadores.ObtenerEntrenadorPorId
{
    public class ObtenerEntrenadorPorIdAD : IObtenerEntrenadorPorIdAD
    {
        private Contexto _elContexto;

        public ObtenerEntrenadorPorIdAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<EntrenadorDto> ObtenerEntrenadorPorId(string identityUserId)
        {

            var entrenador = await _elContexto.Entrenadores
                .Include(e => e.Usuario)
                .Where(e => e.Usuario.identityUserId == identityUserId)
                .Select(e => new EntrenadorDto
                {

                    idEntrenador = e.idEntrenador,

                    idUsuario = e.idUsuario,

                    nombreCompleto =
                    e.Usuario.nombre + " " +
                    e.Usuario.apellido1 + " " +
                    e.Usuario.apellido2,

                    correo = e.Usuario.correo,

                    especialidad = e.especialidad,

                    descripcion = e.descripcion,

                    fechaContratacion = e.fechaContratacion,

                    estado = e.estado

                })
                .FirstOrDefaultAsync();


            return entrenador;

        }
    }
}