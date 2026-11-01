using GimnasioJena.Abstracciones.AccesoADatos.Bitacora;
using GimnasioJena.Abstracciones.General.Fechas;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.AccesoADatos.Bitacora;
using GimnasioJena.LogicaDeNegocio.General.Fechas;

namespace GimnasioJena.LogicaDeNegocio.Bitacora
{
    public class RegistrarBitacoraLN : IRegistrarBitacoraLN
    {
        private readonly IRegistrarBitacoraAD _registrarBitacoraAD;
        private readonly IFechasLN _fechasLN;
        public RegistrarBitacoraLN()
        {
            _registrarBitacoraAD = new RegistrarBitacoraAD();
            _fechasLN = new FechasLN(); 
        }

        public bool RegistrarBitacora(BitacoraDto bitacora)
        {
            if (bitacora == null)
                return false;

            if (string.IsNullOrWhiteSpace(bitacora.tablaAfectada))
                return false;

            if (string.IsNullOrWhiteSpace(bitacora.accionRealizada))
                return false;

            bitacora.fechaAccion = _fechasLN.ObtenerFechaActual();

            return _registrarBitacoraAD.RegistrarBitacora(bitacora) > 0;
        }
    }
}