using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("asistencia_estudiante")]
[Index("IdSesion", "IdEstudiante", Name = "asistencia_estudiante_id_sesion_id_estudiante_key", IsUnique = true)]
public partial class AsistenciaEstudiante
{
    [Key]
    [Column("id_asistencia_est")]
    public int IdAsistenciaEst { get; set; }

    [Column("id_sesion")]
    public int IdSesion { get; set; }

    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Column("estado_asistencia")]
    [StringLength(30)]
    public string EstadoAsistencia { get; set; } = null!;

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; }

    [InverseProperty("IdAsistenciaEstNavigation")]
    public virtual ExcusaInasistencium? ExcusaInasistencium { get; set; }

    [ForeignKey("IdEstudiante")]
    [InverseProperty("AsistenciaEstudiantes")]
    public virtual PerfilEstudiante IdEstudianteNavigation { get; set; } = null!;

    [ForeignKey("IdSesion")]
    [InverseProperty("AsistenciaEstudiantes")]
    public virtual ClaseSesion IdSesionNavigation { get; set; } = null!;
}
