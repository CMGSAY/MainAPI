using System;

namespace MainAPI.Models.DTOs
{
    public class CrearTareaDto
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal PunteoMaximo { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Visibilidad { get; set; }
        public DateTime? FechaAsignacion { get; set; }
    }

    public class CrearMaterialDto
    {
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string UrlDocumento { get; set; } = null!;
        public bool Visibilidad { get; set; }
        public DateTime? FechaAsignacion { get; set; }
    }

    public class CalificarDto
    {
        public decimal PorcentajeObtenido { get; set; }
        public string? Comentarios { get; set; }
    }

    public class CalificacionDto
    {
        public decimal Calificacion { get; set; }
        public string? Comentarios { get; set; }
    }
}
