using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("tarea")]
public partial class Tarea
{
    [Key]
    [Column("id_tarea")]
    public int IdTarea { get; set; }

    [Column("id_curso_habilitado")]
    public int IdCursoHabilitado { get; set; }

    [Column("titulo")]
    [StringLength(150)]
    public string Titulo { get; set; } = null!;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("url_documento_referencia")]
    [StringLength(500)]
    public string? UrlDocumentoReferencia { get; set; }

    [Column("fecha_creacion")]
    public DateOnly? FechaCreacion { get; set; }

    [Column("fecha_vencimiento", TypeName = "timestamp without time zone")]
    public DateTime FechaVencimiento { get; set; }

    [Column("punteo_maximo")]
    [Precision(5, 2)]
    public decimal PunteoMaximo { get; set; }

    [Column("visibilidad")]
    public bool? Visibilidad { get; set; }

    [InverseProperty("IdTareaNavigation")]
    public virtual ICollection<EntregaTarea> EntregaTareas { get; set; } = new List<EntregaTarea>();

    [ForeignKey("IdCursoHabilitado")]
    [InverseProperty("Tareas")]
    public virtual CursoHabilitado IdCursoHabilitadoNavigation { get; set; } = null!;
}
