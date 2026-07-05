using GimnasioJena.Abstracciones.Modelos.Bitacora;

namespace GimnasioJena.Abstracciones.LogicaDeNegocio.Bitacora
{
    public interface IRegistrarBitacoraLN
    {
        bool RegistrarBitacora(BitacoraDto bitacora);
    }
}
