namespace MainAPI.Models.DTOs
{
    public class BitacoraDto
    {
        public int LoginUserId { get; set; }
        public string Accion { get; set; } = null!;
        public string? Ip { get; set; }
    }
}
