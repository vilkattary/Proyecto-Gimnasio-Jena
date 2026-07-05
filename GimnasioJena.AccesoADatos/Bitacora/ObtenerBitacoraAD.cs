using GimnasioJena.Abstracciones.AccesoADatos.Bitacora;
using GimnasioJena.Abstracciones.Modelos.Bitacora;
using System.Collections.Generic;
using System.Linq;

namespace GimnasioJena.AccesoADatos.Bitacora
{
    public class ObtenerBitacoraAD : IObtenerBitacorasAD
    {
        private readonly Contexto _contexto;

        public ObtenerBitacoraAD()
        {
            _contexto = new Contexto();
        }

        public List<BitacoraDto> ObtenerBitacoras()
        {
            var bitacoras =
                (from b in _contexto.Bitacoras
                 join u in _contexto.Usuarios
                    on b.idUsuario equals u.idUsuario into usuarioJoin
                 from u in usuarioJoin.DefaultIfEmpty()
                 select new BitacoraDto
                 {
                     idBitacora = b.idBitacora,
                     idUsuario = b.idUsuario,
                     nombreUsuario = u != null ? u.nombre + " " + u.apellido1 : "Sistema",
                     tablaAfectada = b.tablaAfectada,
                     accionRealizada = b.accionRealizada,
                     idRegistroAfectado = b.idRegistroAfectado,
                     fechaAccion = b.fechaAccion,
                     detalle = b.detalle,
                     ipUsuario = b.ipUsuario
                 })
                .OrderByDescending(b => b.fechaAccion)
                .ToList();

            return bitacoras;
        }
    }
}