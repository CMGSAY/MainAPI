using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("perfil_administrador")]
[Index("Dpi", Name = "perfil_administrador_dpi_key", IsUnique = true)]
[Index("IdPersona", Name = "perfil_administrador_id_persona_key", IsUnique = true)]
public partial class PerfilAdministrador
{
    [Key]
    [Column("id_admin")]
    public int IdAdmin { get; set; }

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

    [ForeignKey("IdMunicipio")]
    [InverseProperty("PerfilAdministradors")]
    public virtual Municipio? IdMunicipioNavigation { get; set; }

    [ForeignKey("IdPersona")]
    [InverseProperty("PerfilAdministrador")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
