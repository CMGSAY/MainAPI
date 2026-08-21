namespace MainAPI.Models.DTOs
{
    public class CursoDto
    {
        public string NombreCurso { get; set; } = null!;
        public int Creditos { get; set; }
        public string? Descripcion { get; set; }
        public decimal? PunteoMaximoTotal { get; set; }
    }
}
