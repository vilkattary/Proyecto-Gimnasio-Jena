using System;

namespace GimnasioJena.Abstracciones.Modelos.Home
{
    public class ContenidoWebDto
    {
        public int Id { get; set; }
        public string Pagina { get; set; }
        public string Seccion { get; set; }
        public string Clave { get; set; }
        public string TextoPrincipal { get; set; }
        public string TextoSecundario { get; set; }
        public string UrlImagen { get; set; }
        public int Orden { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
