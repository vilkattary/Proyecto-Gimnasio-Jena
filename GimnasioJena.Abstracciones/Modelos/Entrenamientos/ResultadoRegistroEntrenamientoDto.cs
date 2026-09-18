namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    // Resultado del guardado de un registro de entrenamiento del cliente.
    public class ResultadoRegistroEntrenamientoDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public bool NuevoRecord { get; set; }
        public int idWorkoutLog { get; set; }
    }
}
