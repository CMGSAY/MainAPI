using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("entrega_tarea")]
[Index("IdTarea", "IdEstudiante", Name = "entrega_tarea_id_tarea_id_estudiante_key", IsUnique = true)]
public partial class EntregaTarea
{
    [Key]
    [Column("id_entrega")]
    public int IdEntrega { get; set; }

    [Column("id_tarea")]
    public int IdTarea { get; set; }

    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Column("url_archivo_adjunto")]
    [StringLength(500)]
    public string? UrlArchivoAdjunto { get; set; }

    [Column("fecha_envio", TypeName = "timestamp without time zone")]
    public DateTime? FechaEnvio { get; set; }

    [Column("calificacion")]
    [Precision(5, 2)]
    public decimal? Calificacion { get; set; }

    [Column("comentarios_catedratico")]
    public string? ComentariosCatedratico { get; set; }

    [ForeignKey("IdEstudiante")]
    [InverseProperty("EntregaTareas")]
    public virtual PerfilEstudiante IdEstudianteNavigation { get; set; } = null!;

    [ForeignKey("IdTarea")]
    [InverseProperty("EntregaTareas")]
    public virtual Tarea IdTareaNavigation { get; set; } = null!;

    [Column("porcentaje_obtenido")]
    public decimal? PorcentajeObtenido { get; set; }

}
