using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("modulo_edificio")]
public partial class ModuloEdificio
{
    [Key]
    [Column("id_modulo")]
    public int IdModulo { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("ubicacion")]
    [StringLength(200)]
    public string? Ubicacion { get; set; }

    [Column("id_sede")]
    public int IdSede { get; set; }

    [InverseProperty("IdModuloNavigation")]
    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();

    [ForeignKey("IdSede")]
    [InverseProperty("ModuloEdificios")]
    public virtual Sede IdSedeNavigation { get; set; } = null!;
}
