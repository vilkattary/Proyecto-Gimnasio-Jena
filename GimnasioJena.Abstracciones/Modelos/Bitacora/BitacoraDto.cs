using System;

namespace GimnasioJena.Abstracciones.Modelos.Bitacora
{
    public class BitacoraDto
    {
        public int idBitacora { get; set; }
        public int? idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string tablaAfectada { get; set; }
        public string accionRealizada { get; set; }
        public int? idRegistroAfectado { get; set; }
        public DateTime fechaAccion { get; set; }
        public string detalle { get; set; }
        public string ipUsuario { get; set; }
    }
}