using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("ciclo_escolar")]
public partial class CicloEscolar
{
    [Key]
    [Column("id_ciclo")]
    public int IdCiclo { get; set; }

    [Column("anio")]
    public int Anio { get; set; }

    [Column("nombre_ciclo")]
    [StringLength(50)]
    public string NombreCiclo { get; set; } = null!;

    [Column("fecha_inicio")]
    public DateOnly? FechaInicio { get; set; }

    [Column("fecha_finalizacion")]
    public DateOnly? FechaFinalizacion { get; set; }

    [Column("estado")]
    public bool? Estado { get; set; }

    [InverseProperty("IdCicloNavigation")]
    public virtual ICollection<CursoHabilitado> CursoHabilitados { get; set; } = new List<CursoHabilitado>();
}
