using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("aula")]
public partial class Aula
{
    [Key]
    [Column("id_aula")]
    public int IdAula { get; set; }

    [Column("numero_salon")]
    [StringLength(20)]
    public string NumeroSalon { get; set; } = null!;

    [Column("id_modulo")]
    public int IdModulo { get; set; }

    [Column("capacidad_alumnos")]
    public int? CapacidadAlumnos { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; } = true;

    [InverseProperty("IdAulaNavigation")]
    public virtual ICollection<CursoHabilitado> CursoHabilitados { get; set; } = new List<CursoHabilitado>();

    [ForeignKey("IdModulo")]
    [InverseProperty("Aulas")]
    public virtual ModuloEdificio IdModuloNavigation { get; set; } = null!;
}
