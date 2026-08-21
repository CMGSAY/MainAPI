using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Models;

[Table("curso_habilitado")]
public partial class CursoHabilitado
{
    [Key]
    [Column("id_curso_habilitado")]
    public int IdCursoHabilitado { get; set; }

    [Column("id_carrera_semestre_curso")]
    public int IdCarreraSemestreCurso { get; set; }

    [Column("id_ciclo")]
    public int IdCiclo { get; set; }

    [Column("id_jornada")]
    public int IdJornada { get; set; }

    [Column("id_seccion")]
    public int IdSeccion { get; set; }

    [Column("id_aula")]
    public int IdAula { get; set; }

    [Column("id_catedratico")]
    public int IdCatedratico { get; set; }

    [Column("estado")]
    [StringLength(30)]
    public string? Estado { get; set; }

    [InverseProperty("IdCursoHabilitadoNavigation")]
    public virtual ICollection<AsignacionCurso> AsignacionCursos { get; set; } = new List<AsignacionCurso>();

    [InverseProperty("IdCursoHabilitadoNavigation")]
    public virtual ICollection<ClaseSesion> ClaseSesions { get; set; } = new List<ClaseSesion>();

    [InverseProperty("IdCursoHabilitadoNavigation")]
    public virtual ICollection<EvaluacionFija> EvaluacionFijas { get; set; } = new List<EvaluacionFija>();

    [ForeignKey("IdAula")]
    [InverseProperty("CursoHabilitados")]
    public virtual Aula IdAulaNavigation { get; set; } = null!;

    [ForeignKey("IdCarreraSemestreCurso")]
    [InverseProperty("CursoHabilitados")]
    public virtual CarreraSemestreCurso IdCarreraSemestreCursoNavigation { get; set; } = null!;

    [ForeignKey("IdCatedratico")]
    [InverseProperty("CursoHabilitados")]
    public virtual PerfilCatedratico IdCatedraticoNavigation { get; set; } = null!;

    [ForeignKey("IdCiclo")]
    [InverseProperty("CursoHabilitados")]
    public virtual CicloEscolar IdCicloNavigation { get; set; } = null!;

    [ForeignKey("IdJornada")]
    [InverseProperty("CursoHabilitados")]
    public virtual Jornadum IdJornadaNavigation { get; set; } = null!;

    [ForeignKey("IdSeccion")]
    [InverseProperty("CursoHabilitados")]
    public virtual Seccion IdSeccionNavigation { get; set; } = null!;

    [InverseProperty("IdCursoHabilitadoNavigation")]
    public virtual ICollection<MaterialClase> MaterialClases { get; set; } = new List<MaterialClase>();

    [InverseProperty("IdCursoHabilitadoNavigation")]
    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
