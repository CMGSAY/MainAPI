using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("curso_prerrequisito")]
public partial class CursoPrerrequisito
{
    [Key]
    [Column("id_prerrequisito")]
    public int IdPrerrequisito { get; set; }

    [Column("id_curso")]
    public int IdCurso { get; set; }

    [Column("id_curso_requerido")]
    public int IdCursoRequerido { get; set; }

    [ForeignKey("IdCurso")]
    [InverseProperty("CursoPrerrequisitoIdCursoNavigations")]
    public virtual Curso IdCursoNavigation { get; set; } = null!;

    [ForeignKey("IdCursoRequerido")]
    [InverseProperty("CursoPrerrequisitoIdCursoRequeridoNavigations")]
    public virtual Curso IdCursoRequeridoNavigation { get; set; } = null!;
}
