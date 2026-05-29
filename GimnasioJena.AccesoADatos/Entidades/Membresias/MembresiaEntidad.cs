using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.Membresias
{
    [Table("MembresiaCliente")]
    public class MembresiaEntidad
    {
        [Key]
        public int idMembresiaCliente { get; set; }

        public int idUsuario { get; set; }
        public int idPlanMembresia { get; set; }
        public int idEstadoMembresia { get; set; }

        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }

        public int? clasesDisponibles { get; set; }

        [StringLength(500)]
        public string observaciones { get; set; }

        public DateTime fechaCreacion { get; set; }
    }
}