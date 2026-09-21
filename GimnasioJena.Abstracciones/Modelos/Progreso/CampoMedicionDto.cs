using System.ComponentModel.DataAnnotations;

namespace GimnasioJena.Abstracciones.Modelos.Progreso
{
    public class CampoMedicionDto
    {
        public int IdCampo { get; set; }

        [Required]
        public string NombreCampo { get; set; }

        public bool Activo { get; set; }
    }
}
