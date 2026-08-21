using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("asistencia_catedratico")]
[Index("IdSesion", "IdCatedratico", Name = "asistencia_catedratico_id_sesion_id_catedratico_key", IsUnique = true)]
public partial class AsistenciaCatedratico
{
    [Key]
    [Column("id_asistencia_cat")]
    public int IdAsistenciaCat { get; set; }

    [Column("id_sesion")]
    public int IdSesion { get; set; }

    [Column("id_catedratico")]
    public int IdCatedratico { get; set; }

    [Column("estado_asistencia")]
    [StringLength(30)]
    public string EstadoAsistencia { get; set; } = null!;

    [Column("fecha_registro", TypeName = "timestamp without time zone")]
    public DateTime? FechaRegistro { get; set; }

    [ForeignKey("IdCatedratico")]
    [InverseProperty("AsistenciaCatedraticos")]
    public virtual PerfilCatedratico IdCatedraticoNavigation { get; set; } = null!;

    [ForeignKey("IdSesion")]
    [InverseProperty("AsistenciaCatedraticos")]
    public virtual ClaseSesion IdSesionNavigation { get; set; } = null!;
}
