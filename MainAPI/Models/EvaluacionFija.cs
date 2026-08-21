using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("evaluacion_fija")]
[Index("IdCursoHabilitado", "TipoEvaluacion", Name = "evaluacion_fija_id_curso_habilitado_tipo_evaluacion_key", IsUnique = true)]
public partial class EvaluacionFija
{
    [Key]
    [Column("id_evaluacion")]
    public int IdEvaluacion { get; set; }

    [Column("id_curso_habilitado")]
    public int IdCursoHabilitado { get; set; }

    [Column("tipo_evaluacion")]
    [StringLength(50)]
    public string TipoEvaluacion { get; set; } = null!;

    [Column("punteo_asignado")]
    [Precision(5, 2)]
    public decimal PunteoAsignado { get; set; }

    [Column("fecha_evaluacion")]
    public DateOnly? FechaEvaluacion { get; set; }

    [InverseProperty("IdEvaluacionNavigation")]
    public virtual ICollection<CalificacionEvaluacion> CalificacionEvaluacions { get; set; } = new List<CalificacionEvaluacion>();

    [ForeignKey("IdCursoHabilitado")]
    [InverseProperty("EvaluacionFijas")]
    public virtual CursoHabilitado IdCursoHabilitadoNavigation { get; set; } = null!;
}
