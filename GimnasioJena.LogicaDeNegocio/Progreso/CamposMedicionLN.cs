using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Progreso;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Progreso
{
    public class CamposMedicionLN : ICamposMedicionLN
    {
        private readonly ICamposMedicionAD _camposMedicionAD;

        public CamposMedicionLN()
        {
            _camposMedicionAD = new CamposMedicionAD();
        }

        public async Task<List<CampoMedicionDto>> ObtenerTodosLN()
        {
            return await _camposMedicionAD.ObtenerTodosAD();
        }

        public async Task CrearLN(CampoMedicionDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información del campo de medición.");
            }

            if (string.IsNullOrWhiteSpace(dto.NombreCampo))
            {
                throw new ArgumentException("Debe indicar el nombre del campo de medición.");
            }

            dto.NombreCampo = dto.NombreCampo.Trim();

            await _camposMedicionAD.CrearAD(dto);
        }

        public async Task CambiarEstadoLN(int idCampo)
        {
            if (idCampo <= 0)
            {
                throw new ArgumentException("El campo de medición indicado no es válido.");
            }

            await _camposMedicionAD.CambiarEstadoAD(idCampo);
        }
    }
}
