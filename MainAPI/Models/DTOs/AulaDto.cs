namespace MainAPI.Models.DTOs
{
    public class AulaDto
    {
        public string NumeroSalon { get; set; } = null!;
        public int IdModulo { get; set; }
        public int? CapacidadAlumnos { get; set; }
    }
}
