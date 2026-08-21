namespace MainAPI.Models.DTOs
{
    public class PersonaDto
    {
        public int LoginUserId { get; set; }
        public string PrimerNombre { get; set; } = null!;
        public string? SegundoNombre { get; set; }
        public string? TercerNombre { get; set; }
        public string PrimerApellido { get; set; } = null!;
        public string? SegundoApellido { get; set; }
    }
}
