namespace MainAPI.Models.DTOs
{
    public class MaterialDto
    {
        public int IdCursoHabilitado { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? TipoArchivo { get; set; }
        public string UrlDocumento { get; set; } = null!;
    }
}
