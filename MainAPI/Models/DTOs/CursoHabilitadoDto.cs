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
        //public TimeOnly? HorarioInicio { get; set; }
        //public TimeOnly? HorarioFin { get; set; }
        public List<HorarioCursoDto> Horarios { get; set; } = new List<HorarioCursoDto>();
        public string? Estado { get; set; }
    }
}
