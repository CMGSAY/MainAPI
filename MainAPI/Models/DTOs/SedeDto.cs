namespace MainAPI.Models.DTOs
{
    public class SedeDto
    {
        public string Nombre { get; set; } = null!;
        public string? UbicacionExacta { get; set; }
        public string? Zona { get; set; }
        public int? IdMunicipio { get; set; }
        public string? TelefonoContactoPrincipal { get; set; }
    }
}
