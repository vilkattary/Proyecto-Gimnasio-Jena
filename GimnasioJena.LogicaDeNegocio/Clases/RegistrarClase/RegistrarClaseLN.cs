using GimnasioJena.Abstracciones.AccesoADatos.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Clases.RegistrarClase;
using GimnasioJena.LogicaDeNegocio.General.Fechas;
using System;
using System.Linq;

namespace GimnasioJena.LogicaDeNegocio.Clases.RegistrarClase
{
    public class RegistrarClaseLN : IRegistrarClaseLN
    {
        private IRegistrarClaseAD _registrarClaseAD;
        private IFechasLN         _fechasLN;

        public RegistrarClaseLN()
        {
            _registrarClaseAD = new RegistrarClaseAD();
            _fechasLN         = new FechasLN();
        }

        public bool RegistrarClase(ClaseCrearDto dto)
        {
            if (dto == null)
                return false;

            dto.fechaCreacion = _fechasLN.ObtenerFechaActual();

            if (dto.Horarios != null && dto.Horarios.Any())
            {
                foreach (var h in dto.Horarios)
                {
                    if (string.IsNullOrWhiteSpace(h.fechaClase)  ||
                        string.IsNullOrWhiteSpace(h.horaInicio)  ||
                        string.IsNullOrWhiteSpace(h.horaFin)     ||
                        h.idUsuarioEntrenador <= 0)
                        return false;

                    if (TimeSpan.Parse(h.horaFin) <= TimeSpan.Parse(h.horaInicio))
                        return false;
                }
            }

            return _registrarClaseAD.RegistrarClase(dto) > 0;
        }
    }
}
