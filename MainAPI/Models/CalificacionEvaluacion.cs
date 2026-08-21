using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("calificacion_evaluacion")]
[Index("IdEvaluacion", "IdEstudiante", Name = "calificacion_evaluacion_id_evaluacion_id_estudiante_key", IsUnique = true)]
public partial class CalificacionEvaluacion
{
    [Key]
    [Column("id_calificacion")]
    public int IdCalificacion { get; set; }

    [Column("id_evaluacion")]
    public int IdEvaluacion { get; set; }

    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Column("nota_obtenida")]
    [Precision(5, 2)]
    public decimal? NotaObtenida { get; set; }

    [ForeignKey("IdEstudiante")]
    [InverseProperty("CalificacionEvaluacions")]
    public virtual PerfilEstudiante IdEstudianteNavigation { get; set; } = null!;

    [ForeignKey("IdEvaluacion")]
    [InverseProperty("CalificacionEvaluacions")]
    public virtual EvaluacionFija IdEvaluacionNavigation { get; set; } = null!;
}
