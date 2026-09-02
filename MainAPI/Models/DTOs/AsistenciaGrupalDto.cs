using System;
using System.Collections.Generic;

namespace MainAPI.Models.DTOs
{
    public class AsistenciaGrupalDto
    {
        public DateTime FechaSesion { get; set; }
        public List<AsistenciaEstudianteItemDto> Estudiantes { get; set; } = new();
    }

    public class AsistenciaEstudianteItemDto
    {
        public int IdEstudiante { get; set; }
        public bool IsPresente { get; set; }
    }
}
