using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("persona")]
[Index("LoginUserId", Name = "persona_login_user_id_key", IsUnique = true)]
public partial class Persona
{
    [Key]
    [Column("id_persona")]
    public int IdPersona { get; set; }

    [Column("login_user_id")]
    public int LoginUserId { get; set; }

    [Column("primer_nombre")]
    [StringLength(100)]
    public string PrimerNombre { get; set; } = null!;

    [Column("segundo_nombre")]
    [StringLength(100)]
    public string? SegundoNombre { get; set; }

    [Column("tercer_nombre")]
    [StringLength(100)]
    public string? TercerNombre { get; set; }

    [Column("primer_apellido")]
    [StringLength(100)]
    public string PrimerApellido { get; set; } = null!;

    [Column("segundo_apellido")]
    [StringLength(100)]
    public string? SegundoApellido { get; set; }

    [Column("activo")]
    public bool? Activo { get; set; } = true;

    [InverseProperty("IdPersonaNavigation")]
    public virtual PerfilAdministrador? PerfilAdministrador { get; set; }

    [InverseProperty("IdPersonaNavigation")]
    public virtual PerfilCatedratico? PerfilCatedratico { get; set; }

    [InverseProperty("IdPersonaNavigation")]
    public virtual PerfilEstudiante? PerfilEstudiante { get; set; }
}
