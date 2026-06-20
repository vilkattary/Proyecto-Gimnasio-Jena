using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GimnasioJena.AccesoADatos.Entidades.EstadoReservas
{
    [Table("EstadoReservas")]
    public class EstadoReservaEntidad
    {
        public int idEstadoReserva { get; set; }
        public string nombreEstado { get; set; } = string.Empty;
    }
}
