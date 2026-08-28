namespace MainAPI.Models.DTOs
{
    public class HorarioCursoDto
    {
        public string DiaSemana { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
    }

}
