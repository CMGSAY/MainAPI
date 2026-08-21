namespace MainAPI.Models.DTOs
{
    public class PerfilCatedraticoDto
    {
        public int IdPersona { get; set; }
        public string Dpi { get; set; } = null!;
        public string? NumeroColegiadoActivo { get; set; }
        public string? TelefonoPrincipal { get; set; }
        public string? DireccionCalleAvenida { get; set; }
        public string? Zona { get; set; }
        public int? IdMunicipio { get; set; }
        public string? Especialidad { get; set; }
        public DateOnly? FechaContratacion { get; set; }
    }
}
