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

    [InverseProperty("IdSeccionNavigation")]
    public virtual ICollection<CursoHabilitado> CursoHabilitados { get; set; } = new List<CursoHabilitado>();
}
