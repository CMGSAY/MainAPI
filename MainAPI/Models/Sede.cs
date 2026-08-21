using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("sede")]
public partial class Sede
{
    [Key]
    [Column("id_sede")]
    public int IdSede { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("ubicacion_exacta")]
    [StringLength(200)]
    public string? UbicacionExacta { get; set; }

    [Column("zona")]
    [StringLength(10)]
    public string? Zona { get; set; }

    [Column("id_municipio")]
    public int? IdMunicipio { get; set; }

    [Column("telefono_contacto_principal")]
    [StringLength(15)]
    public string? TelefonoContactoPrincipal { get; set; }

    [InverseProperty("IdSedeNavigation")]
    public virtual ICollection<Facultad> Facultads { get; set; } = new List<Facultad>();

    [ForeignKey("IdMunicipio")]
    [InverseProperty("Sedes")]
    public virtual Municipio? IdMunicipioNavigation { get; set; }

    [InverseProperty("IdSedeNavigation")]
    public virtual ICollection<ModuloEdificio> ModuloEdificios { get; set; } = new List<ModuloEdificio>();
}
