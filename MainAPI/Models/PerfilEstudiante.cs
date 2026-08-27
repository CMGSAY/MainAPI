using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("perfil_estudiante")]
[Index("Carnet", Name = "perfil_estudiante_carnet_key", IsUnique = true)]
[Index("IdPersona", Name = "perfil_estudiante_id_persona_key", IsUnique = true)]
public partial class PerfilEstudiante
{
    [Key]
    [Column("id_estudiante")]
    public int IdEstudiante { get; set; }

    [Column("id_persona")]
    public int IdPersona { get; set; }

    [Column("carnet")]
    [StringLength(20)]
    public string Carnet { get; set; } = null!;

    [Column("telefono_principal")]
    [StringLength(15)]
    public string? TelefonoPrincipal { get; set; }

    [Column("direccion_calle_avenida")]
    [StringLength(200)]
    public string? DireccionCalleAvenida { get; set; }

    [Column("zona")]
    [StringLength(10)]
    public string? Zona { get; set; }

    [Column("id_municipio")]
    public int? IdMunicipio { get; set; }

    [Column("fecha_ingreso")]
    public DateOnly? FechaIngreso { get; set; }

    [InverseProperty("IdEstudianteNavigation")]
    public virtual ICollection<AsignacionCurso> AsignacionCursos { get; set; } = new List<AsignacionCurso>();

    [InverseProperty("IdEstudianteNavigation")]
    public virtual ICollection<AsistenciaEstudiante> AsistenciaEstudiantes { get; set; } = new List<AsistenciaEstudiante>();

    [InverseProperty("IdEstudianteNavigation")]
    public virtual ICollection<CalificacionEvaluacion> CalificacionEvaluacions { get; set; } = new List<CalificacionEvaluacion>();

    [InverseProperty("IdEstudianteNavigation")]
    public virtual ICollection<EntregaTarea> EntregaTareas { get; set; } = new List<EntregaTarea>();

    [ForeignKey("IdMunicipio")]
    [InverseProperty("PerfilEstudiantes")]
    public virtual Municipio? IdMunicipioNavigation { get; set; }

    [ForeignKey("IdPersona")]
    [InverseProperty("PerfilEstudiante")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    [Column("id_semestre_actual")]
    public int? IdSemestreActual { get; set; }
}
}

