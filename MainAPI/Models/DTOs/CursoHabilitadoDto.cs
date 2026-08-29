namespace MainAPI.Models.DTOs
{
    public class CursoHabilitadoDto
    {
        public int IdCarreraSemestreCurso { get; set; }
        public int IdCiclo { get; set; }
        public int IdJornada { get; set; }
        public int IdSeccion { get; set; }
        public int IdAula { get; set; }
        public int IdCatedratico { get; set; }
        public string? Estado { get; set; }
        public List<HorarioCursoDto> Horarios { get; set; } = new List<HorarioCursoDto>();
    }
}
