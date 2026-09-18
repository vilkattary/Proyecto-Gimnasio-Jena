namespace GimnasioJena.Abstracciones.Modelos.Entrenamientos
{
    // Payload que envía el cliente al registrar una medición de biometría.
    public class RegistrarBiometriaDto
    {
        // Se fuerza en el controlador con el usuario autenticado (no confiable desde el cliente).
        public int UserId { get; set; }

        // Formato yyyy-MM-dd. Si viene vacío, el LN usa la fecha de hoy.
        public string MeasurementDate { get; set; }

        public decimal WeightKg { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? MuscleMassPercentage { get; set; }
    }

    // Resultado de la operación de registro de biometría.
    public class ResultadoBiometriaDto
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public int idBiometria { get; set; }
    }
}
