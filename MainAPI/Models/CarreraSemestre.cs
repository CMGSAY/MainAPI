using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("carrera_semestre")]
[Index("IdCarrera", "IdSemestre", Name = "carrera_semestre_id_carrera_id_semestre_key", IsUnique = true)]
public partial class CarreraSemestre
{
    [Key]
    [Column("id_carrera_semestre")]
    public int IdCarreraSemestre { get; set; }

    [Column("id_carrera")]
    public int IdCarrera { get; set; }

    [Column("id_semestre")]
    public int IdSemestre { get; set; }

    [InverseProperty("IdCarreraSemestreNavigation")]
    public virtual ICollection<CarreraSemestreCurso> CarreraSemestreCursos { get; set; } = new List<CarreraSemestreCurso>();

    [ForeignKey("IdCarrera")]
    [InverseProperty("CarreraSemestres")]
    public virtual Carrera IdCarreraNavigation { get; set; } = null!;

    [ForeignKey("IdSemestre")]
    [InverseProperty("CarreraSemestres")]
    public virtual Semestre IdSemestreNavigation { get; set; } = null!;
}
