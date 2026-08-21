namespace MainAPI.Models.DTOs
{
    public class JornadaDto
    {
        public string NombreJornada { get; set; } = null!;
        public TimeOnly? HoraInicio { get; set; }
        public TimeOnly? HoraFin { get; set; }
    }
}
