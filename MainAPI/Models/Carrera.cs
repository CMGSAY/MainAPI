using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("carrera")]
public partial class Carrera
{
    [Key]
    [Column("id_carrera")]
    public int IdCarrera { get; set; }

    [Column("nombre_carrera")]
    [StringLength(150)]
    public string NombreCarrera { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("id_facultad")]
    public int IdFacultad { get; set; }

    [Column("cantidad_semestres")]
    public int? CantidadSemestres { get; set; }

    [Column("creditos_totales")]
    public int? CreditosTotales { get; set; }

    [Column("activa")]
    public bool? Activa { get; set; }

    [InverseProperty("IdCarreraNavigation")]
    public virtual ICollection<CarreraSemestre> CarreraSemestres { get; set; } = new List<CarreraSemestre>();

    [ForeignKey("IdFacultad")]
    [InverseProperty("Carreras")]
    public virtual Facultad IdFacultadNavigation { get; set; } = null!;

    [InverseProperty("IdCarreraNavigation")]
    public virtual ICollection<PerfilEstudiante> PerfilEstudiantes { get; set; } = new List<PerfilEstudiante>();

    [InverseProperty("IdCarreraNavigation")]
    public virtual ICollection<Seccion> Seccions { get; set; } = new List<Seccion>();
}
