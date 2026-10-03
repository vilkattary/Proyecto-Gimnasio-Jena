using GimnasioJena.AccesoADatos.Entidades.Home;
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
using GimnasioJena.AccesoADatos.Entidades.HorariosSemanales;
using GimnasioJena.AccesoADatos.Entidades.Progreso;
using GimnasioJena.AccesoADatos.Entidades.Entrenamientos;

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
        public DbSet<ContenidoWeb> ContenidoWeb { get; set; }
        public DbSet<HorarioSemanalEntidad> HorariosSemanales { get; set; }
        public DbSet<EntrenamientoEntidad> Entrenamientos { get; set; }
        public DbSet<ProgresoClienteEntidad> ProgresosCliente { get; set; }
        public DbSet<CampoMedicionEntidad> CamposMedicion { get; set; }
        public DbSet<RegistroMedicionEntidad> RegistrosMedicion { get; set; }

        public DbSet<MesocicloEntidad> Mesociclos { get; set; }
        public DbSet<PlantillaDiaEntrenamientoEntidad> PlantillasDiaEntrenamiento { get; set; }
        public DbSet<EjercicioEntrenamientoEntidad> EjerciciosEntrenamiento { get; set; }
        public DbSet<ProgresionEjercicioEntidad> ProgresionesEjercicio { get; set; }

        public DbSet<UserWorkoutLogEntidad> UserWorkoutLogs { get; set; }
        public DbSet<UserSetLogEntidad> UserSetLogs { get; set; }
        public DbSet<UserBiometricsEntidad> UserBiometrics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProgresoClienteEntidad>()
                .Property(p => p.PesoAlcanzado)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ProgresoClienteEntidad>()
                .Property(p => p.DistanciaKm)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ProgresoClienteEntidad>()
                .Property(p => p.TiempoMinutos)
                .HasPrecision(10, 2);

            modelBuilder.Entity<RegistroMedicionEntidad>()
                .Property(r => r.Valor)
                .HasPrecision(10, 2);
        }
    }
}
