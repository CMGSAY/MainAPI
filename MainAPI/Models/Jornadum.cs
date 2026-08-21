using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("jornada")]
public partial class Jornadum
{
    [Key]
    [Column("id_jornada")]
    public int IdJornada { get; set; }

    [Column("nombre_jornada")]
    [StringLength(50)]
    public string NombreJornada { get; set; } = null!;

    [Column("hora_inicio")]
    public TimeOnly? HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly? HoraFin { get; set; }

    [InverseProperty("IdJornadaNavigation")]
    public virtual ICollection<CursoHabilitado> CursoHabilitados { get; set; } = new List<CursoHabilitado>();
}
