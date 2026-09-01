using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("departamento")]
public partial class Departamento
{
    [Key]
    [Column("id_departamento")]
    public int IdDepartamento { get; set; }

    [Column("nombre_departamento")]
    [StringLength(100)]
    public string NombreDepartamento { get; set; } = null!;

    [Column("activo")]
    public bool? Activo { get; set; } = true;

    [InverseProperty("IdDepartamentoNavigation")]
    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
}
