namespace MainAPI.Models.DTOs
{
    public class EvaluacionFijaDto
    {
        public int IdCursoHabilitado { get; set; }
        public string TipoEvaluacion { get; set; } = null!;
        public decimal PunteoAsignado { get; set; }
        public DateOnly? FechaEvaluacion { get; set; }
    }
}
