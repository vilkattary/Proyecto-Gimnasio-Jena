using GimnasioJena.Abstracciones.AccesoADatos.Bitacora;
using GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.AccesoADatos.Bitacora;

namespace GimnasioJena.LogicaDeNegocio.Bitacora
{
    public class RegistrarBitacoraLN : IRegistrarBitacoraLN
    {
        private readonly IRegistrarBitacoraAD _registrarBitacoraAD;

        public RegistrarBitacoraLN()
        {
            _registrarBitacoraAD = new RegistrarBitacoraAD();
        }

        public bool RegistrarBitacora(BitacoraDto bitacora)
        {
            if (bitacora == null)
                return false;

            if (string.IsNullOrWhiteSpace(bitacora.tablaAfectada))
                return false;

            if (string.IsNullOrWhiteSpace(bitacora.accionRealizada))
                return false;

            return _registrarBitacoraAD.RegistrarBitacora(bitacora) > 0;
        }
    }
}