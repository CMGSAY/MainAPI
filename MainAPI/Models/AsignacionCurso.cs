using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("asignacion_curso")]
[Index("IdEstudiante", "IdCursoHabilitado", Name = "asignacion_curso_id_estudiante_id_curso_habilitado_key", IsUnique = true)]
public partial class AsignacionCurso
{
    [Key]
    [Column("id_asignacion")]
    public int IdAsignacion { get; set; }

    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Column("id_curso_habilitado")]
    public int IdCursoHabilitado { get; set; }

    [Column("fecha_asignacion")]
    public DateOnly FechaAsignacion { get; set; }

    [Column("estado")]
    [StringLength(30)]
    public string? Estado { get; set; }

    [Column("nota_final")]
    [Precision(5, 2)]
    public decimal? NotaFinal { get; set; }

    [ForeignKey("IdCursoHabilitado")]
    [InverseProperty("AsignacionCursos")]
    public virtual CursoHabilitado IdCursoHabilitadoNavigation { get; set; } = null!;

    [ForeignKey("IdEstudiante")]
    [InverseProperty("AsignacionCursos")]
    public virtual PerfilEstudiante IdEstudianteNavigation { get; set; } = null!;
}
