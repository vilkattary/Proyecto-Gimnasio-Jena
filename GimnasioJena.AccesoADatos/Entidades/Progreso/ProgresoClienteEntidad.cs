using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GimnasioJena.AccesoADatos.Entidades.Clases;

namespace GimnasioJena.AccesoADatos.Entidades.Progreso
{
    [Table("ProgresoCliente")]
    public class ProgresoClienteEntidad
    {
        [Key]
        public int IdProgreso { get; set; }

        public int idUsuario { get; set; }

        public int IdEntrenamiento { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? PesoAlcanzado { get; set; }

        public int? Repeticiones { get; set; }

        public int? Series { get; set; }

        public int? RIR { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? DistanciaKm { get; set; }

        [Column(TypeName = "decimal")]
        public decimal? TiempoMinutos { get; set; }

        public int? Pulsaciones { get; set; }

        public int? NivelEnergia { get; set; }

        [StringLength(1000)]
        public string Notas { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime FechaRegistro { get; set; }

        [StringLength(150)]
        public string NombreEjercicio { get; set; }

        [ForeignKey("ClaseProgramada")]
        public int? idClaseProgramada { get; set; }
        public virtual ClaseEntidad ClaseProgramada { get; set; }
    }
}
