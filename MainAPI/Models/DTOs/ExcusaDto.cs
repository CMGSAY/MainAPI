namespace MainAPI.Models.DTOs
{
    public class ExcusaDto
    {
        public int IdAsistenciaEst { get; set; }
        public string Motivo { get; set; } = null!;
        public string? UrlComprobante { get; set; }
    }
}
