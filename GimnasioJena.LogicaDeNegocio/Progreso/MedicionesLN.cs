using GimnasioJena.Abstracciones.AccesoADatos.Progreso;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Progreso;
using GimnasioJena.Abstracciones.Modelos.Progreso;
using GimnasioJena.AccesoADatos.Progreso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GimnasioJena.LogicaDeNegocio.Progreso
{
    public class MedicionesLN : IMedicionesLN
    {
        private readonly IMedicionesAD _medicionesAD;
        private readonly ICamposMedicionAD _camposMedicionAD;
        private readonly IUsuarioActualAD _usuarioActualAD;

        public MedicionesLN()
        {
            _medicionesAD = new MedicionesAD();
            _camposMedicionAD = new CamposMedicionAD();
            _usuarioActualAD = new UsuarioActualAD();
        }

        public async Task<RegistroMedicionMultipleDto> GenerarFormularioLN(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new ArgumentException("No se pudo identificar al usuario.");
            }

            var idUsuario = await _usuarioActualAD.ObtenerIdUsuarioPorIdentityAD(identityUserId);

            if (idUsuario == null || idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no está registrado en el sistema.");
            }

            var nombreCliente = await _medicionesAD.ObtenerNombreClienteAD(idUsuario.Value);
            var campos = await _camposMedicionAD.ObtenerTodosAD();

            var dto = new RegistroMedicionMultipleDto
            {
                idUsuario = identityUserId,
                NombreCliente = nombreCliente,
                Valores = campos
                    .Where(c => c.Activo)
                    .Select(c => new ValorMedicionDto
                    {
                        IdCampo = c.IdCampo,
                        NombreCampo = c.NombreCampo,
                        Valor = null
                    })
                    .ToList()
            };

            return dto;
        }

        public async Task GuardarMedicionesLN(RegistroMedicionMultipleDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("No se recibió la información de las mediciones.");
            }

            if (string.IsNullOrWhiteSpace(dto.idUsuario))
            {
                throw new ArgumentException("No se pudo identificar al usuario.");
            }

            var idUsuario = await _usuarioActualAD.ObtenerIdUsuarioPorIdentityAD(dto.idUsuario);

            if (idUsuario == null || idUsuario <= 0)
            {
                throw new ArgumentException("El usuario indicado no está registrado en el sistema.");
            }

            var valoresValidos = (dto.Valores ?? new List<ValorMedicionDto>())
                .Where(v => v.Valor.HasValue && v.IdCampo > 0)
                .ToList();

            if (!valoresValidos.Any())
            {
                throw new ArgumentException("Debe ingresar al menos una medición.");
            }

            await _medicionesAD.GuardarMedicionesAD(idUsuario.Value, valoresValidos);
        }
    }
}
