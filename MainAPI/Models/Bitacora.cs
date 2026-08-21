using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("bitacora")]
public partial class Bitacora
{
    [Key]
    [Column("id_bitacora")]
    public int IdBitacora { get; set; }

    [Column("login_user_id")]
    public int LoginUserId { get; set; }

    [Column("accion")]
    [StringLength(255)]
    public string Accion { get; set; } = null!;

    [Column("fecha_hora", TypeName = "timestamp without time zone")]
    public DateTime? FechaHora { get; set; }

    [Column("ip")]
    [StringLength(50)]
    public string? Ip { get; set; }
}
