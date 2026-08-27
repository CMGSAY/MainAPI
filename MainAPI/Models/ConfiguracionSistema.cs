using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainAPI.Models
{
    [Table("configuracion_sistema")]
    public class ConfiguracionSistema
    {
        [Key]
        [Column("id_config")]
        public int IdConfig { get; set; }

        [Column("clave")]
        public string Clave { get; set; } = null!;

        [Column("valor")]
        public string Valor { get; set; } = null!;
    }
}