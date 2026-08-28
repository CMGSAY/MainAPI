using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("configuracion_sistema")]
[Index("Clave", Name = "configuracion_sistema_clave_key", IsUnique = true)]
public partial class ConfiguracionSistema
{
    [Key]
    [Column("id_config")]
    public int IdConfig { get; set; }

    [Column("clave")]
    [StringLength(50)]
    public string Clave { get; set; } = null!;

    [Column("valor")]
    [StringLength(255)]
    public string Valor { get; set; } = null!;
}
