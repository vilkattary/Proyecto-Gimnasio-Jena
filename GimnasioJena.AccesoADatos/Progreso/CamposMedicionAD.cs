using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Progreso
{
    public class CamposMedicionAD : ICamposMedicionAD
    {
        private readonly Contexto _elContexto;

        public CamposMedicionAD()
        {
            _elContexto = new Contexto();
        }

        public async Task<List<CampoMedicionDto>> ObtenerTodosAD()
        {
            var campos =
                await (from c in _elContexto.CamposMedicion
                       orderby c.NombreCampo
                       select new CampoMedicionDto
                       {
                           IdCampo = c.IdCampo,
                           NombreCampo = c.NombreCampo,
                           Activo = c.Activo
                       })
                    .ToListAsync();

            return campos;
        }

        public async Task CrearAD(CampoMedicionDto dto)
        {
            var nuevoCampo = new CampoMedicionEntidad
            {
                NombreCampo = dto.NombreCampo,
                Activo = true
            };

            _elContexto.CamposMedicion.Add(nuevoCampo);
            await _elContexto.SaveChangesAsync();
        }

        public async Task CambiarEstadoAD(int idCampo)
        {
            var campo = await _elContexto.CamposMedicion
                .FirstOrDefaultAsync(c => c.IdCampo == idCampo);

            if (campo != null)
            {
                campo.Activo = !campo.Activo;
                await _elContexto.SaveChangesAsync();
            }
        }
    }
}
