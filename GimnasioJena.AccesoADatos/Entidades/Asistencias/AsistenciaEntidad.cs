using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.Asistencias
{
    [Table("Asistencia")]
    public class AsistenciaEntidad
    {
        [Key]
        public int idAsistencia { get; set; }

        public int idReserva { get; set; }

        public int? idUsuarioRecepcionista { get; set; }

        public DateTime fechaRegistro { get; set; }

        public bool asistio { get; set; }

        [StringLength(500)]
        public string observaciones { get; set; }
    }
}