using System.Web;

namespace GimnasioJena.Abstracciones.Modelos.Home
{
    public class ModificarSeccionHomeDto
    {
        public int Id { get; set; }

        public string TextoPrincipal { get; set; }

        public string TextoSecundario { get; set; }

        public string UrlImagen { get; set; }

        public HttpPostedFileBase ArchivoImagen { get; set; }
    }
}
