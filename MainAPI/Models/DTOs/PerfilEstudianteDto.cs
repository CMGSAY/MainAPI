namespace MainAPI.Models.DTOs
{
    public class PerfilEstudianteDto
    {
        public int IdPersona { get; set; }
        public string Carnet { get; set; } = null!;
        public string? TelefonoPrincipal { get; set; }
        public string? DireccionCalleAvenida { get; set; }
        public string? Zona { get; set; }
        public int? IdMunicipio { get; set; }
        public DateOnly? FechaIngreso { get; set; }
    }
}
