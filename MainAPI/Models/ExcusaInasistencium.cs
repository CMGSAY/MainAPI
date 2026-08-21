using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("excusa_inasistencia")]
[Index("IdAsistenciaEst", Name = "excusa_inasistencia_id_asistencia_est_key", IsUnique = true)]
public partial class ExcusaInasistencium
{
    [Key]
    [Column("id_excusa")]
    public int IdExcusa { get; set; }

    [Column("id_asistencia_est")]
    public int IdAsistenciaEst { get; set; }

    [Column("motivo")]
    public string Motivo { get; set; } = null!;

    [Column("url_comprobante")]
    [StringLength(500)]
    public string? UrlComprobante { get; set; }

    [Column("estado_aprobacion")]
    [StringLength(30)]
    public string? EstadoAprobacion { get; set; }

    [Column("fecha_solicitud", TypeName = "timestamp without time zone")]
    public DateTime? FechaSolicitud { get; set; }

    [ForeignKey("IdAsistenciaEst")]
    [InverseProperty("ExcusaInasistencium")]
    public virtual AsistenciaEstudiante IdAsistenciaEstNavigation { get; set; } = null!;
}
