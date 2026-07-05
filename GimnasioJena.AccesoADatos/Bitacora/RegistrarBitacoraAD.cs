using GimnasioJena.Abstracciones.AccesoADatos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using GimnasioJena.AccesoADatos.Entidades.Bitacora;
using System;

namespace GimnasioJena.AccesoADatos.Bitacora
{
    public class RegistrarBitacoraAD : IRegistrarBitacoraAD
    {
        private readonly Contexto _contexto;

        public RegistrarBitacoraAD()
        {
            _contexto = new Contexto();
        }

        public int RegistrarBitacora(BitacoraDto bitacora)
        {
            var entidad = new BitacoraEntidad
            {
                idUsuario = bitacora.idUsuario,
                tablaAfectada = bitacora.tablaAfectada,
                accionRealizada = bitacora.accionRealizada,
                idRegistroAfectado = bitacora.idRegistroAfectado,
                fechaAccion = DateTime.Now,
                detalle = bitacora.detalle,
                ipUsuario = bitacora.ipUsuario
            };

            _contexto.Bitacoras.Add(entidad);
            return _contexto.SaveChanges();
        }
    }
}