using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.Bitacora
{
    [Table("Bitacora")]
    public class BitacoraEntidad
    {
        [Key]
        public int idBitacora { get; set; }

        public int? idUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string tablaAfectada { get; set; }

        [Required]
        [StringLength(30)]
        public string accionRealizada { get; set; }

        public int? idRegistroAfectado { get; set; }

        public DateTime fechaAccion { get; set; }

        public string detalle { get; set; }

        [StringLength(50)]
        public string ipUsuario { get; set; }
    }
}