using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.Reservas
{
    [Table("Reserva")]
    public class ReservaEntidad
    {
        [Key]
        public int idReserva { get; set; }

        public int idUsuario { get; set; }
        public int idClaseProgramada { get; set; }
        public int idEstadoReserva { get; set; }

        public DateTime fechaReserva { get; set; }

        [StringLength(500)]
        public string observaciones { get; set; }
    }
}