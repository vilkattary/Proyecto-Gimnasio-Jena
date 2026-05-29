using GimnasioJena.AccesoADatos.Entidades.Asistencias;
using GimnasioJena.AccesoADatos.Entidades.Bitacora;
using GimnasioJena.AccesoADatos.Entidades.Catalogos;
using GimnasioJena.AccesoADatos.Entidades.Clases;
using GimnasioJena.AccesoADatos.Entidades.Comunicacion;
using GimnasioJena.AccesoADatos.Entidades.Entrenadores;
using GimnasioJena.AccesoADatos.Entidades.Membresias;
using GimnasioJena.AccesoADatos.Entidades.Pagos;
using GimnasioJena.AccesoADatos.Entidades.Reservas;
using GimnasioJena.AccesoADatos.Entidades.Roles;
using GimnasioJena.AccesoADatos.Entidades.Usuarios;
using System.Data.Entity;

namespace GimnasioJena.AccesoADatos
{
    public class Contexto : DbContext
    {

        public Contexto() : base("name=Contexto")
        {

        }
        public DbSet<RolEntidad> Roles { get; set; }
        public DbSet<UsuarioEntidad> Usuarios { get; set; }
        public DbSet<EntrenadorEntidad> Entrenadores { get; set; }
        public DbSet<MembresiaEntidad> Membresias { get; set; }
        public DbSet<ClaseEntidad> Clases { get; set; }
        public DbSet<ReservaEntidad> Reservas { get; set; }
        public DbSet<AsistenciaEntidad> Asistencias { get; set; }
        public DbSet<MensajeEntidad> Mensajes { get; set; }
        public DbSet<BitacoraEntidad> Bitacoras { get; set; }
        public DbSet<EstadoMembresiaEntidad> EstadoMembresias { get; set; }
        public DbSet<EstadoClaseEntidad> EstadoClases { get; set; }
        public DbSet<EstadoReservaEntidad> EstadoReservas { get; set; }
        public DbSet<EstadoPagoEntidad> EstadoPagos { get; set; }
        public DbSet<MetodoPagoEntidad> MetodoPagos { get; set; }
        public DbSet<TipoClaseEntidad> TiposClase { get; set; }
        public DbSet<PlanMembresiaEntidad> PlanesMembresia { get; set; }
        public DbSet<PagoEntidad> Pagos { get; set; }
    }
}
