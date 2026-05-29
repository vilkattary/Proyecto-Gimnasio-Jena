namespace GimnasioJena.Abstracciones.Modelos.Reportes
{
    public class DashboardGeneralDto
    {
        public int totalUsuarios { get; set; }
        public int totalClientes { get; set; }
        public int totalEntrenadores { get; set; }
        public int membresiasActivas { get; set; }
        public int reservasActivas { get; set; }
        public int clasesProgramadas { get; set; }
        public decimal ingresosMes { get; set; }
    }
}