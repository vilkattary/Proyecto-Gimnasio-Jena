using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using System;
using System.Collections.Generic;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class MedicionesAD : IMedicionesAD
    {
        private readonly Contexto _elContexto;

        public MedicionesAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<List<MedicionDinamicaDto>> ObtenerMedicionesUsuarioAD(int idUsuario)
        {
            var mediciones =
                await (from r in _elContexto.RegistrosMedicion
                       join c in _elContexto.CamposMedicion
                          on r.IdCampo equals c.IdCampo
                       where r.idUsuario == idUsuario
                          && c.Activo
                       orderby r.FechaRegistro
                       select new MedicionDinamicaDto
                       {
                           IdCampo = r.IdCampo,
                           NombreCampo = c.NombreCampo,
                           Valor = r.Valor,
                           FechaRegistro = r.FechaRegistro
                       })
                    .ToListAsync();

            return mediciones;
        }

        public async Task<string> ObtenerNombreClienteAD(int idUsuario)
        {
            var nombre =
                await (from u in _elContexto.Usuarios
                       where u.idUsuario == idUsuario
                       select (u.nombre + " " + u.apellido1))
                    .FirstOrDefaultAsync();

            return nombre;
        }

        public async Task GuardarMedicionesAD(int idUsuario, List<ValorMedicionDto> valores)
        {
            foreach (var valor in valores)
            {
                var registro = new RegistroMedicionEntidad
                {
                    idUsuario = idUsuario,
                    IdCampo = valor.IdCampo,
                    Valor = valor.Valor.Value,
                    FechaRegistro = DateTime.Now
                };

                _elContexto.RegistrosMedicion.Add(registro);
            }

            await _elContexto.SaveChangesAsync();
        }
    }
}
