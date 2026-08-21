using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("municipio")]
public partial class Municipio
{
    [Key]
    [Column("id_municipio")]
    public int IdMunicipio { get; set; }

    [Column("id_departamento")]
    public int IdDepartamento { get; set; }

    [Column("nombre_municipio")]
    [StringLength(100)]
    public string NombreMunicipio { get; set; } = null!;

    [ForeignKey("IdDepartamento")]
    [InverseProperty("Municipios")]
    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    [InverseProperty("IdMunicipioNavigation")]
    public virtual ICollection<PerfilAdministrador> PerfilAdministradors { get; set; } = new List<PerfilAdministrador>();

    [InverseProperty("IdMunicipioNavigation")]
    public virtual ICollection<PerfilCatedratico> PerfilCatedraticos { get; set; } = new List<PerfilCatedratico>();

    [InverseProperty("IdMunicipioNavigation")]
    public virtual ICollection<PerfilEstudiante> PerfilEstudiantes { get; set; } = new List<PerfilEstudiante>();

    [InverseProperty("IdMunicipioNavigation")]
    public virtual ICollection<Sede> Sedes { get; set; } = new List<Sede>();
}
