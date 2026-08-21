namespace MainAPI.Models.DTOs
{
    public class CarreraDto
    {
        public string NombreCarrera { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int IdFacultad { get; set; }
        public int? CantidadSemestres { get; set; }
        public int? CreditosTotales { get; set; }
        public bool? Activa { get; set; }
    }
}
