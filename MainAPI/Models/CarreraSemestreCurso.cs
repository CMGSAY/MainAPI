using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("carrera_semestre_curso")]
[Index("IdCarreraSemestre", "IdCurso", Name = "carrera_semestre_curso_id_carrera_semestre_id_curso_key", IsUnique = true)]
public partial class CarreraSemestreCurso
{
    [Key]
    [Column("id_carrera_semestre_curso")]
    public int IdCarreraSemestreCurso { get; set; }

    [Column("id_carrera_semestre")]
    public int IdCarreraSemestre { get; set; }

    [Column("id_curso")]
    public int IdCurso { get; set; }

    [InverseProperty("IdCarreraSemestreCursoNavigation")]
    public virtual ICollection<CursoHabilitado> CursoHabilitados { get; set; } = new List<CursoHabilitado>();

    [ForeignKey("IdCarreraSemestre")]
    [InverseProperty("CarreraSemestreCursos")]
    public virtual CarreraSemestre IdCarreraSemestreNavigation { get; set; } = null!;

    [ForeignKey("IdCurso")]
    [InverseProperty("CarreraSemestreCursos")]
    public virtual Curso IdCursoNavigation { get; set; } = null!;
}
