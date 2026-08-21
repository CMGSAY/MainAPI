namespace MainAPI.Models.DTOs
{
    public class FacultadDto
    {
        public string NombreFacultad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int IdSede { get; set; }
    }
}
