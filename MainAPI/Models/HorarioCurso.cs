using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainAPI.Models
{
    [Table("horario_curso")]
    public partial class HorarioCurso
    {
        [Key]
        [Column("id_horario")]
        public int IdHorario { get; set; }

        [Column("id_curso_habilitado")]
        public int IdCursoHabilitado { get; set; }

        [Required]
        [Column("dia_semana")]
        [StringLength(20)]
        public string DiaSemana { get; set; } = null!;

        [Column("hora_inicio")]
        public TimeOnly HoraInicio { get; set; }

        [Column("hora_fin")]
        public TimeOnly HoraFin { get; set; }

        [ForeignKey("IdCursoHabilitado")]
        [InverseProperty("HorarioCursos")]
        public virtual CursoHabilitado IdCursoHabilitadoNavigation { get; set; } = null!;
    }
}
