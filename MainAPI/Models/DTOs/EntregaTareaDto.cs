namespace MainAPI.Models.DTOs
{
    public class EntregaTareaDto
    {
        public int IdTarea { get; set; }
        public int IdEstudiante { get; set; }
        public string? UrlArchivoAdjunto { get; set; }
    }
}
