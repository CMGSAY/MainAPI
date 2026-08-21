using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("clase_sesion")]
public partial class ClaseSesion
{
    [Key]
    [Column("id_sesion")]
    public int IdSesion { get; set; }

    [Column("id_curso_habilitado")]
    public int IdCursoHabilitado { get; set; }

    [Column("fecha_sesion")]
    public DateOnly FechaSesion { get; set; }

    [Column("hora_inicio")]
    public TimeOnly? HoraInicio { get; set; }

    [Column("hora_fin")]
    public TimeOnly? HoraFin { get; set; }

    [Column("tema_impartido")]
    [StringLength(200)]
    public string? TemaImpartido { get; set; }

    [InverseProperty("IdSesionNavigation")]
    public virtual ICollection<AsistenciaCatedratico> AsistenciaCatedraticos { get; set; } = new List<AsistenciaCatedratico>();

    [InverseProperty("IdSesionNavigation")]
    public virtual ICollection<AsistenciaEstudiante> AsistenciaEstudiantes { get; set; } = new List<AsistenciaEstudiante>();

    [ForeignKey("IdCursoHabilitado")]
    [InverseProperty("ClaseSesions")]
    public virtual CursoHabilitado IdCursoHabilitadoNavigation { get; set; } = null!;
}
