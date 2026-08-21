using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("facultad")]
public partial class Facultad
{
    [Key]
    [Column("id_facultad")]
    public int IdFacultad { get; set; }

    [Column("nombre_facultad")]
    [StringLength(100)]
    public string NombreFacultad { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("id_sede")]
    public int IdSede { get; set; }

    [InverseProperty("IdFacultadNavigation")]
    public virtual ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();

    [ForeignKey("IdSede")]
    [InverseProperty("Facultads")]
    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
