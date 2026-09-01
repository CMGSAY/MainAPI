using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("semestre")]
public partial class Semestre
{
    [Key]
    [Column("id_semestre")]
    public int IdSemestre { get; set; }

    [Column("nombre_semestre")]
    [StringLength(50)]
    public string NombreSemestre { get; set; } = null!;

    [Column("numero_orden")]
    public int NumeroOrden { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; } = true;

    [InverseProperty("IdSemestreNavigation")]
    public virtual ICollection<CarreraSemestre> CarreraSemestres { get; set; } = new List<CarreraSemestre>();

    [InverseProperty("IdSemestreActualNavigation")]
    public virtual ICollection<PerfilEstudiante> PerfilEstudiantes { get; set; } = new List<PerfilEstudiante>();

    [InverseProperty("IdSemestreNavigation")]
    public virtual ICollection<Seccion> Seccions { get; set; } = new List<Seccion>();
}
