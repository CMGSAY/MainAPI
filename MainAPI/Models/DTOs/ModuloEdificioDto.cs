namespace MainAPI.Models.DTOs
{
    public class ModuloEdificioDto
    {
        public string Nombre { get; set; } = null!;
        public string? Ubicacion { get; set; }
        public int IdSede { get; set; }
    }
}
