using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GimnasioJena.AccesoADatos.Entidades.Membresias
{
    [Table("PlanMembresia")]
    public class PlanMembresiaEntidad
    {
        [Key]
        public int idPlanMembresia { get; set; }

        [Required]
        [StringLength(100)]
        public string nombrePlan { get; set; }

        [StringLength(500)]
        public string descripcion { get; set; }

        public decimal precio { get; set; }

        public int? cantidadClases { get; set; }

        public int duracionDias { get; set; }

        public bool incluyeClasePrueba { get; set; }

        public bool estado { get; set; }

        public DateTime fechaCreacion { get; set; }
    }
}