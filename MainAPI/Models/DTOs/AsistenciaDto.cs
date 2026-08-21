namespace MainAPI.Models.DTOs
{
    public class AsistenciaDto
    {
        public int IdSesion { get; set; }
        public int IdEstudiante { get; set; }
        public string EstadoAsistencia { get; set; } = null!;
    }
}
