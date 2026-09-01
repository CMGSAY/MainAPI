using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("perfil_catedratico")]
[Index("Dpi", Name = "perfil_catedratico_dpi_key", IsUnique = true)]
[Index("IdPersona", Name = "perfil_catedratico_id_persona_key", IsUnique = true)]
public partial class PerfilCatedratico
{
    [Key]
    [Column("id_catedratico")]
    public int IdCatedratico { get; set; }

    [Column("id_persona")]
    public int IdPersona { get; set; }

    [Column("dpi")]
    [StringLength(20)]
    public string Dpi { get; set; } = null!;

    [Column("numero_colegiado_activo")]
    [StringLength(50)]
    public string? NumeroColegiadoActivo { get; set; }

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

    [Column("especialidad")]
    [StringLength(100)]
    public string? Especialidad { get; set; }

    [Column("fecha_contratacion")]
    public DateOnly? FechaContratacion { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; } = true;

    [InverseProperty("IdCatedraticoNavigation")]
    public virtual ICollection<AsistenciaCatedratico> AsistenciaCatedraticos { get; set; } = new List<AsistenciaCatedratico>();

    [InverseProperty("IdCatedraticoNavigation")]
    public virtual ICollection<CursoHabilitado> CursoHabilitados { get; set; } = new List<CursoHabilitado>();

    [ForeignKey("IdMunicipio")]
    [InverseProperty("PerfilCatedraticos")]
    public virtual Municipio? IdMunicipioNavigation { get; set; }

    [ForeignKey("IdPersona")]
    [InverseProperty("PerfilCatedratico")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
