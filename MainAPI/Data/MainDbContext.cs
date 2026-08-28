using System;
using System.Collections.Generic;
using MainAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MainAPI.Data;

public partial class MainDbContext : DbContext
{
    public MainDbContext()
    {
    }

    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AsignacionCurso> AsignacionCursos { get; set; }

    public virtual DbSet<AsistenciaCatedratico> AsistenciaCatedraticos { get; set; }

    public virtual DbSet<AsistenciaEstudiante> AsistenciaEstudiantes { get; set; }

    public virtual DbSet<Aula> Aulas { get; set; }

    public virtual DbSet<Bitacora> Bitacoras { get; set; }

    public virtual DbSet<CalificacionEvaluacion> CalificacionEvaluacions { get; set; }

    public virtual DbSet<Carrera> Carreras { get; set; }

    public virtual DbSet<CarreraSemestre> CarreraSemestres { get; set; }

    public virtual DbSet<CarreraSemestreCurso> CarreraSemestreCursos { get; set; }

    public virtual DbSet<CicloEscolar> CicloEscolars { get; set; }

    public virtual DbSet<ClaseSesion> ClaseSesions { get; set; }

    public virtual DbSet<ConfiguracionSistema> ConfiguracionSistemas { get; set; }

    public virtual DbSet<Curso> Cursos { get; set; }

    public virtual DbSet<CursoHabilitado> CursoHabilitados { get; set; }

    public virtual DbSet<CursoPrerrequisito> CursoPrerrequisitos { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<EntregaTarea> EntregaTareas { get; set; }

    public virtual DbSet<EvaluacionFija> EvaluacionFijas { get; set; }

    public virtual DbSet<ExcusaInasistencium> ExcusaInasistencia { get; set; }

    public virtual DbSet<Facultad> Facultads { get; set; }

    public virtual DbSet<Invitation> Invitations { get; set; }

    public virtual DbSet<Jornadum> Jornada { get; set; }

    public virtual DbSet<Jwk> Jwks { get; set; }

    public virtual DbSet<MaterialClase> MaterialClases { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<ModuloEdificio> ModuloEdificios { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<PerfilAdministrador> PerfilAdministradors { get; set; }

    public virtual DbSet<PerfilCatedratico> PerfilCatedraticos { get; set; }

    public virtual DbSet<PerfilEstudiante> PerfilEstudiantes { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<ProjectConfig> ProjectConfigs { get; set; }

    public virtual DbSet<Seccion> Seccions { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Semestre> Semestres { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<HorarioCurso> HorarioCursos { get; set; }

    public virtual DbSet<Verification> Verifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-long-cake-axo1o4uu-pooler.c-4.us-east-2.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_DqBP6LHRJWk3; SSL Mode=VerifyFull; Channel Binding=Require;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_session_jwt");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts).HasConstraintName("account_userId_fkey");
        });

        modelBuilder.Entity<AsignacionCurso>(entity =>
        {
            entity.HasKey(e => e.IdAsignacion).HasName("asignacion_curso_pkey");

            entity.Property(e => e.Estado).HasDefaultValueSql("'asignado'::character varying");
            entity.Property(e => e.NotaFinal).HasDefaultValue(0.00m);

            entity.HasOne(d => d.IdCursoHabilitadoNavigation).WithMany(p => p.AsignacionCursos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asignacion_curso_id_curso_habilitado_fkey");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.AsignacionCursos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asignacion_curso_id_estudiante_fkey");
        });

        modelBuilder.Entity<AsistenciaCatedratico>(entity =>
        {
            entity.HasKey(e => e.IdAsistenciaCat).HasName("asistencia_catedratico_pkey");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdCatedraticoNavigation).WithMany(p => p.AsistenciaCatedraticos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asistencia_catedratico_id_catedratico_fkey");

            entity.HasOne(d => d.IdSesionNavigation).WithMany(p => p.AsistenciaCatedraticos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asistencia_catedratico_id_sesion_fkey");
        });

        modelBuilder.Entity<AsistenciaEstudiante>(entity =>
        {
            entity.HasKey(e => e.IdAsistenciaEst).HasName("asistencia_estudiante_pkey");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.AsistenciaEstudiantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asistencia_estudiante_id_estudiante_fkey");

            entity.HasOne(d => d.IdSesionNavigation).WithMany(p => p.AsistenciaEstudiantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asistencia_estudiante_id_sesion_fkey");
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.HasKey(e => e.IdAula).HasName("aula_pkey");

            entity.HasOne(d => d.IdModuloNavigation).WithMany(p => p.Aulas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("aula_id_modulo_fkey");
        });

        modelBuilder.Entity<Bitacora>(entity =>
        {
            entity.HasKey(e => e.IdBitacora).HasName("bitacora_pkey");

            entity.Property(e => e.FechaHora).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<CalificacionEvaluacion>(entity =>
        {
            entity.HasKey(e => e.IdCalificacion).HasName("calificacion_evaluacion_pkey");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.CalificacionEvaluacions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("calificacion_evaluacion_id_estudiante_fkey");

            entity.HasOne(d => d.IdEvaluacionNavigation).WithMany(p => p.CalificacionEvaluacions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("calificacion_evaluacion_id_evaluacion_fkey");
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.IdCarrera).HasName("carrera_pkey");

            entity.Property(e => e.Activa).HasDefaultValue(true);

            entity.HasOne(d => d.IdFacultadNavigation).WithMany(p => p.Carreras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrera_id_facultad_fkey");
        });

        modelBuilder.Entity<CarreraSemestre>(entity =>
        {
            entity.HasKey(e => e.IdCarreraSemestre).HasName("carrera_semestre_pkey");

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.CarreraSemestres)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrera_semestre_id_carrera_fkey");

            entity.HasOne(d => d.IdSemestreNavigation).WithMany(p => p.CarreraSemestres)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrera_semestre_id_semestre_fkey");
        });

        modelBuilder.Entity<CarreraSemestreCurso>(entity =>
        {
            entity.HasKey(e => e.IdCarreraSemestreCurso).HasName("carrera_semestre_curso_pkey");

            entity.HasOne(d => d.IdCarreraSemestreNavigation).WithMany(p => p.CarreraSemestreCursos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrera_semestre_curso_id_carrera_semestre_fkey");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.CarreraSemestreCursos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("carrera_semestre_curso_id_curso_fkey");
        });

        modelBuilder.Entity<CicloEscolar>(entity =>
        {
            entity.HasKey(e => e.IdCiclo).HasName("ciclo_escolar_pkey");

            entity.Property(e => e.Estado).HasDefaultValue(true);
        });

        modelBuilder.Entity<ClaseSesion>(entity =>
        {
            entity.HasKey(e => e.IdSesion).HasName("clase_sesion_pkey");

            entity.HasOne(d => d.IdCursoHabilitadoNavigation).WithMany(p => p.ClaseSesions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clase_sesion_id_curso_habilitado_fkey");
        });

        modelBuilder.Entity<ConfiguracionSistema>(entity =>
        {
            entity.HasKey(e => e.IdConfig).HasName("configuracion_sistema_pkey");
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("curso_pkey");

            entity.Property(e => e.PunteoMaximoTotal).HasDefaultValue(100.00m);
        });

        modelBuilder.Entity<CursoHabilitado>(entity =>
        {
            entity.HasKey(e => e.IdCursoHabilitado).HasName("curso_habilitado_pkey");

            entity.Property(e => e.Estado).HasDefaultValueSql("'activo'::character varying");

            entity.HasOne(d => d.IdAulaNavigation).WithMany(p => p.CursoHabilitados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_habilitado_id_aula_fkey");

            entity.HasOne(d => d.IdCarreraSemestreCursoNavigation).WithMany(p => p.CursoHabilitados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_habilitado_id_carrera_semestre_curso_fkey");

            entity.HasOne(d => d.IdCatedraticoNavigation).WithMany(p => p.CursoHabilitados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_habilitado_id_catedratico_fkey");

            entity.HasOne(d => d.IdCicloNavigation).WithMany(p => p.CursoHabilitados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_habilitado_id_ciclo_fkey");

            entity.HasOne(d => d.IdJornadaNavigation).WithMany(p => p.CursoHabilitados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_habilitado_id_jornada_fkey");

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.CursoHabilitados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_habilitado_id_seccion_fkey");
        });

        modelBuilder.Entity<CursoPrerrequisito>(entity =>
        {
            entity.HasKey(e => e.IdPrerrequisito).HasName("curso_prerrequisito_pkey");

            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.CursoPrerrequisitoIdCursoNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_prerrequisito_id_curso_fkey");

            entity.HasOne(d => d.IdCursoRequeridoNavigation).WithMany(p => p.CursoPrerrequisitoIdCursoRequeridoNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("curso_prerrequisito_id_curso_requerido_fkey");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("departamento_pkey");
        });

        modelBuilder.Entity<EntregaTarea>(entity =>
        {
            entity.HasKey(e => e.IdEntrega).HasName("entrega_tarea_pkey");

            entity.HasOne(d => d.IdEstudianteNavigation).WithMany(p => p.EntregaTareas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrega_tarea_id_estudiante_fkey");

            entity.HasOne(d => d.IdTareaNavigation).WithMany(p => p.EntregaTareas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrega_tarea_id_tarea_fkey");
        });

        modelBuilder.Entity<EvaluacionFija>(entity =>
        {
            entity.HasKey(e => e.IdEvaluacion).HasName("evaluacion_fija_pkey");

            entity.HasOne(d => d.IdCursoHabilitadoNavigation).WithMany(p => p.EvaluacionFijas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("evaluacion_fija_id_curso_habilitado_fkey");
        });

        modelBuilder.Entity<ExcusaInasistencium>(entity =>
        {
            entity.HasKey(e => e.IdExcusa).HasName("excusa_inasistencia_pkey");

            entity.Property(e => e.EstadoAprobacion).HasDefaultValueSql("'Pendiente'::character varying");
            entity.Property(e => e.FechaSolicitud).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdAsistenciaEstNavigation).WithOne(p => p.ExcusaInasistencium)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("excusa_inasistencia_id_asistencia_est_fkey");
        });

        modelBuilder.Entity<Facultad>(entity =>
        {
            entity.HasKey(e => e.IdFacultad).HasName("facultad_pkey");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Facultads)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("facultad_id_sede_fkey");
        });

        modelBuilder.Entity<Invitation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invitation_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Inviter).WithMany(p => p.Invitations).HasConstraintName("invitation_inviterId_fkey");

            entity.HasOne(d => d.Organization).WithMany(p => p.Invitations).HasConstraintName("invitation_organizationId_fkey");
        });

        modelBuilder.Entity<Jornadum>(entity =>
        {
            entity.HasKey(e => e.IdJornada).HasName("jornada_pkey");
        });

        modelBuilder.Entity<Jwk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("jwks_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<MaterialClase>(entity =>
        {
            entity.HasKey(e => e.IdMaterial).HasName("material_clase_pkey");

            entity.Property(e => e.FechaSubida).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.Visibilidad).HasDefaultValue(true);

            entity.HasOne(d => d.IdCursoHabilitadoNavigation).WithMany(p => p.MaterialClases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("material_clase_id_curso_habilitado_fkey");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("member_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Organization).WithMany(p => p.Members).HasConstraintName("member_organizationId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Members).HasConstraintName("member_userId_fkey");
        });

        modelBuilder.Entity<ModuloEdificio>(entity =>
        {
            entity.HasKey(e => e.IdModulo).HasName("modulo_edificio_pkey");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.ModuloEdificios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("modulo_edificio_id_sede_fkey");
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.HasKey(e => e.IdMunicipio).HasName("municipio_pkey");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Municipios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("municipio_id_departamento_fkey");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organization_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        });

        modelBuilder.Entity<PerfilAdministrador>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("perfil_administrador_pkey");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.PerfilAdministradors).HasConstraintName("perfil_administrador_id_municipio_fkey");

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.PerfilAdministrador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("perfil_administrador_id_persona_fkey");
        });

        modelBuilder.Entity<PerfilCatedratico>(entity =>
        {
            entity.HasKey(e => e.IdCatedratico).HasName("perfil_catedratico_pkey");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.PerfilCatedraticos).HasConstraintName("perfil_catedratico_id_municipio_fkey");

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.PerfilCatedratico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("perfil_catedratico_id_persona_fkey");
        });

        modelBuilder.Entity<PerfilEstudiante>(entity =>
        {
            entity.HasKey(e => e.IdEstudiante).HasName("perfil_estudiante_pkey");

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.PerfilEstudiantes).HasConstraintName("fk_perfil_estudiante_carrera");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.PerfilEstudiantes).HasConstraintName("perfil_estudiante_id_municipio_fkey");

            entity.HasOne(d => d.IdPersonaNavigation).WithOne(p => p.PerfilEstudiante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("perfil_estudiante_id_persona_fkey");

            entity.HasOne(d => d.IdSemestreActualNavigation).WithMany(p => p.PerfilEstudiantes).HasConstraintName("fk_perfil_estudiante_semestre");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("persona_pkey");
        });

        modelBuilder.Entity<ProjectConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_config_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Seccion>(entity =>
        {
            entity.HasKey(e => e.IdSeccion).HasName("seccion_pkey");

            entity.HasOne(d => d.IdCarreraNavigation).WithMany(p => p.Seccions).HasConstraintName("fk_seccion_carrera");

            entity.HasOne(d => d.IdSemestreNavigation).WithMany(p => p.Seccions).HasConstraintName("fk_seccion_semestre");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("sede_pkey");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.Sedes).HasConstraintName("sede_id_municipio_fkey");
        });

        modelBuilder.Entity<Semestre>(entity =>
        {
            entity.HasKey(e => e.IdSemestre).HasName("semestre_pkey");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("session_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions).HasConstraintName("session_userId_fkey");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("tarea_pkey");

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.Visibilidad).HasDefaultValue(true);

            entity.HasOne(d => d.IdCursoHabilitadoNavigation).WithMany(p => p.Tareas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tarea_id_curso_habilitado_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Verification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("verification_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
