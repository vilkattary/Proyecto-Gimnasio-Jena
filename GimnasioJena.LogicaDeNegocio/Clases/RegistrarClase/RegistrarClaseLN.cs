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
        private  IRegistrarClaseAD _registrarClaseAD;
        private  IFechasLN _fechasLN;

        public RegistrarClaseLN()
        {
            _registrarClaseAD = new RegistrarClaseAD();
            _fechasLN = new FechasLN();
        }

        public bool RegistrarClase(ClaseCrearDto RegistrarClaseAGuardar)
        {
            try
            {

                RegistrarClaseAGuardar.fechaCreacion = RegistrarClaseAGuardar.fechaCreacion; _fechasLN.ObtenerFechaActual();

                int cantidadDeDatosAlmacenados = _registrarClaseAD.RegistrarClase(RegistrarClaseAGuardar);

                return cantidadDeDatosAlmacenados > 0;
            }
            catch (Exception)
            {
               
                throw;
            }
        }
    }
}
