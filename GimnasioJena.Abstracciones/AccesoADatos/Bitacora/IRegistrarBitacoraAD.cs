using GimnasioJena.Abstracciones.Modelos.Bitacora;

namespace GimnasioJena.Abstracciones.AccesoADatos.Bitacora
{
    public interface IRegistrarBitacoraAD
    {
        int RegistrarBitacora(BitacoraDto bitacora);
    }
}