namespace MainAPI.Models.DTOs
{
    public class CalificacionEvaluacionDto
    {
        public int IdEvaluacion { get; set; }
        public int IdEstudiante { get; set; }
        public decimal? NotaObtenida { get; set; }
    }
}
