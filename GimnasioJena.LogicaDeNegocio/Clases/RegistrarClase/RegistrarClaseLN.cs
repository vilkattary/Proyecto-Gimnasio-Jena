using GimnasioJena.Abstracciones.AccesoADatos.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Clases.RegistrarClase;
using GimnasioJena.Abstracciones.Modelos.Clases;
using GimnasioJena.AccesoADatos.Clases.RegistrarClase;
using GimnasioJena.LogicaDeNegocio.General.Fechas;
using System;

namespace GimnasioJena.LogicaDeNegocio.Clases.RegistrarClase
{
    public class RegistrarClaseLN : IRegistrarClaseLN
    {
        private IRegistrarClaseAD _registrarClaseAD;
        private IFechasLN _fechasLN;

        public RegistrarClaseLN()
        {
            _registrarClaseAD = new RegistrarClaseAD();
            _fechasLN = new FechasLN();
        }

        public bool RegistrarClase(ClaseCrearDto registrarClaseAGuardar)
        {
            if (registrarClaseAGuardar == null)
            {
                return false;
            }

            DateTime fechaActual =
                _fechasLN.ObtenerFechaActual();

            DateTime fechaHoraClase =
                registrarClaseAGuardar.fechaClase.Date
                .Add(registrarClaseAGuardar.horaInicio);

            if (fechaHoraClase <= fechaActual)
            {
                throw new ArgumentException(
                    "No se puede programar una clase en una fecha " +
                    "u hora que ya pasó. Seleccione un horario futuro."
                );
            }

            registrarClaseAGuardar.fechaCreacion =
                fechaActual;

            int cantidadDeDatosAlmacenados =
                _registrarClaseAD.RegistrarClase(registrarClaseAGuardar);

            return cantidadDeDatosAlmacenados > 0;
        }
    }
}