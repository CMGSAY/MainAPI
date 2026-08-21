using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("material_clase")]
public partial class MaterialClase
{
    [Key]
    [Column("id_material")]
    public int IdMaterial { get; set; }

    [Column("id_curso_habilitado")]
    public int IdCursoHabilitado { get; set; }

    [Column("titulo")]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("tipo_archivo")]
    [StringLength(50)]
    public string? TipoArchivo { get; set; }

    [Column("url_documento")]
    [StringLength(500)]
    public string UrlDocumento { get; set; } = null!;

    [Column("fecha_subida")]
    public DateOnly? FechaSubida { get; set; }

    [Column("visibilidad")]
    public bool? Visibilidad { get; set; }

    [ForeignKey("IdCursoHabilitado")]
    [InverseProperty("MaterialClases")]
    public virtual CursoHabilitado IdCursoHabilitadoNavigation { get; set; } = null!;
}
