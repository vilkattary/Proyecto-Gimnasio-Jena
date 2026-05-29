using System;

namespace GimnasioJena.Abstracciones.Modelos.Roles
{
    public class RolListadoDto
    {
        public int idRol { get; set; }
        public string nombreRol { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
