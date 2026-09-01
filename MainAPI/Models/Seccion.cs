using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("seccion")]
public partial class Seccion
{
    [Key]
    [Column("id_seccion")]
    public int IdSeccion { get; set; }

    [Column("nombre_seccion")]
    [StringLength(10)]
    public string NombreSeccion { get; set; } = null!;

    [Column("cupo_maximo")]
    public int? CupoMaximo { get; set; }

    [Column("id_carrera")]
    public int? IdCarrera { get; set; }

    [Column("id_semestre")]
    public int? IdSemestre { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; } = true;

    [InverseProperty("IdSeccionNavigation")]
    public virtual ICollection<CursoHabilitado> CursoHabilitados { get; set; } = new List<CursoHabilitado>();

    [ForeignKey("IdCarrera")]
    public virtual Carrera? IdCarreraNavigation { get; set; }

    [ForeignKey("IdSemestre")]
    public virtual Semestre? IdSemestreNavigation { get; set; }
}