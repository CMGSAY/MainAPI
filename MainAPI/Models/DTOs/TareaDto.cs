namespace MainAPI.Models.DTOs
{
    public class TareaDto
    {
        public int IdCursoHabilitado { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? UrlDocumentoReferencia { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal PunteoMaximo { get; set; }
    }
}
