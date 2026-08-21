namespace MainAPI.Models.DTOs
{
    public class SesionDto
    {
        public int IdCursoHabilitado { get; set; }
        public DateOnly FechaSesion { get; set; }
        public TimeOnly? HoraInicio { get; set; }
        public TimeOnly? HoraFin { get; set; }
        public string? TemaImpartido { get; set; }
    }
}
