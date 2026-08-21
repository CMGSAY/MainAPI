using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("curso")]
public partial class Curso
{
    [Key]
    [Column("id_curso")]
    public int IdCurso { get; set; }

    [Column("nombre_curso")]
    [StringLength(150)]
    public string NombreCurso { get; set; } = null!;

    [Column("creditos")]
    public int Creditos { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("punteo_maximo_total")]
    [Precision(5, 2)]
    public decimal? PunteoMaximoTotal { get; set; }

    [InverseProperty("IdCursoNavigation")]
    public virtual ICollection<CarreraSemestreCurso> CarreraSemestreCursos { get; set; } = new List<CarreraSemestreCurso>();

    [InverseProperty("IdCursoNavigation")]
    public virtual ICollection<CursoPrerrequisito> CursoPrerrequisitoIdCursoNavigations { get; set; } = new List<CursoPrerrequisito>();

    [InverseProperty("IdCursoRequeridoNavigation")]
    public virtual ICollection<CursoPrerrequisito> CursoPrerrequisitoIdCursoRequeridoNavigations { get; set; } = new List<CursoPrerrequisito>();
}
