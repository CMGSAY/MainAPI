namespace MainAPI.Models.DTOs
{
    public class CicloEscolarDto
    {
        public int Anio { get; set; }
        public string NombreCiclo { get; set; } = null!;
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFinalizacion { get; set; }
        public bool? Estado { get; set; }
    }
}
