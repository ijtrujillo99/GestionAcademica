using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Entidades.Cat;
using GestionAcademica.Api.Infraestructura.Entidades.Eval;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Infraestructura.Persistencia;

public class GestionAcademicaDbContext : DbContext
{
    public GestionAcademicaDbContext(DbContextOptions<GestionAcademicaDbContext> options)
        : base(options)
    {
    }

    public DbSet<AreaConocimiento> AreasConocimiento => Set<AreaConocimiento>();
    public DbSet<Carrera> Carreras => Set<Carrera>();
    public DbSet<PlanEstudio> PlanesEstudio => Set<PlanEstudio>();
    public DbSet<Asignatura> Asignaturas => Set<Asignatura>();
    public DbSet<PlanEstudioDetalle> PlanesEstudioDetalle => Set<PlanEstudioDetalle>();
    public DbSet<Docente> Docentes => Set<Docente>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Aula> Aulas => Set<Aula>();
    public DbSet<PeriodoAcademico> PeriodosAcademicos => Set<PeriodoAcademico>();
    public DbSet<Grupo> Grupos => Set<Grupo>();
    public DbSet<CursoOfertado> CursosOfertados => Set<CursoOfertado>();
    public DbSet<Horario> Horarios => Set<Horario>();
    public DbSet<InscripcionCurso> Inscripciones => Set<InscripcionCurso>();
    public DbSet<Clase> Clases => Set<Clase>();
    public DbSet<Asistencia> Asistencias => Set<Asistencia>();

    public DbSet<EstadoInscripcion> EstadosInscripcion => Set<EstadoInscripcion>();
    public DbSet<EstadoAsistencia> EstadosAsistencia => Set<EstadoAsistencia>();
    public DbSet<TipoEvaluacion> TiposEvaluacion => Set<TipoEvaluacion>();

    public DbSet<Parcial> Parciales => Set<Parcial>();
    public DbSet<Evaluacion> Evaluaciones => Set<Evaluacion>();
    public DbSet<Calificacion> Calificaciones => Set<Calificacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigurarAcademico(modelBuilder);
        ConfigurarCatalogos(modelBuilder);
        ConfigurarEvaluacion(modelBuilder);
    }

    private static void ConfigurarAcademico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AreaConocimiento>(entity =>
        {
            entity.ToTable("AreaConocimiento", Esquemas.Acad);
            entity.HasKey(e => e.IdAreaConocimiento);
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(300);
        });

        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.ToTable("Carrera", Esquemas.Acad);
            entity.HasKey(e => e.IdCarrera);
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.HasOne(e => e.AreaConocimiento)
                .WithMany(a => a.Carreras)
                .HasForeignKey(e => e.IdAreaConocimiento);
        });

        modelBuilder.Entity<PlanEstudio>(entity =>
        {
            entity.ToTable("PlanEstudio", Esquemas.Acad);
            entity.HasKey(e => e.IdPlanEstudio);
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.HasOne(e => e.Carrera)
                .WithMany(c => c.PlanesEstudio)
                .HasForeignKey(e => e.IdCarrera);
        });

        modelBuilder.Entity<Asignatura>(entity =>
        {
            entity.ToTable("Asignatura", Esquemas.Acad);
            entity.HasKey(e => e.IdAsignatura);
            entity.Property(e => e.Codigo).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(300);
        });

        modelBuilder.Entity<PlanEstudioDetalle>(entity =>
        {
            entity.ToTable("PlanEstudioDetalle", Esquemas.Acad);
            entity.HasKey(e => e.IdPlanDetalle);
            entity.HasOne(e => e.PlanEstudio)
                .WithMany(p => p.Detalles)
                .HasForeignKey(e => e.IdPlanEstudio);
            entity.HasOne(e => e.Asignatura)
                .WithMany(a => a.Planes)
                .HasForeignKey(e => e.IdAsignatura);
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.ToTable("Docente", Esquemas.Acad);
            entity.HasKey(e => e.IdDocente);
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Correo).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Telefono).HasMaxLength(30);
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.ToTable("Estudiante", Esquemas.Acad);
            entity.HasKey(e => e.IdEstudiante);
            entity.Property(e => e.Carnet).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Sexo).HasMaxLength(1).IsRequired().IsFixedLength();
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(30);
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
        });

        modelBuilder.Entity<Aula>(entity =>
        {
            entity.ToTable("Aula", Esquemas.Acad);
            entity.HasKey(e => e.IdAula);
            entity.Property(e => e.CodigoAula).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Ubicacion).HasMaxLength(150);
        });

        modelBuilder.Entity<PeriodoAcademico>(entity =>
        {
            entity.ToTable("PeriodoAcademico", Esquemas.Acad);
            entity.HasKey(e => e.IdPeriodo);
            entity.Property(e => e.FechaInicio).HasColumnType("date");
            entity.Property(e => e.FechaFin).HasColumnType("date");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.ToTable("Grupo", Esquemas.Acad);
            entity.HasKey(e => e.IdGrupo);
            entity.Property(e => e.CodigoGrupo).HasMaxLength(30).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.HasOne(e => e.Periodo)
                .WithMany(p => p.Grupos)
                .HasForeignKey(e => e.IdPeriodo);
            entity.HasOne(e => e.Carrera)
                .WithMany(c => c.Grupos)
                .HasForeignKey(e => e.IdCarrera);
        });

        modelBuilder.Entity<CursoOfertado>(entity =>
        {
            entity.ToTable("CursoOfertado", Esquemas.Acad);
            entity.HasKey(e => e.IdCursoOfertado);
            entity.HasOne(e => e.Asignatura)
                .WithMany(a => a.CursosOfertados)
                .HasForeignKey(e => e.IdAsignatura);
            entity.HasOne(e => e.Grupo)
                .WithMany(g => g.CursosOfertados)
                .HasForeignKey(e => e.IdGrupo);
            entity.HasOne(e => e.Docente)
                .WithMany(d => d.CursosOfertados)
                .HasForeignKey(e => e.IdDocente);
            entity.HasOne(e => e.Aula)
                .WithMany(a => a.CursosOfertados)
                .HasForeignKey(e => e.IdAula);
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.ToTable("Horario", Esquemas.Acad);
            entity.HasKey(e => e.IdHorario);
            entity.Property(e => e.HoraInicio).HasColumnType("time(0)");
            entity.Property(e => e.HoraFin).HasColumnType("time(0)");
            entity.HasOne(e => e.CursoOfertado)
                .WithMany(c => c.Horarios)
                .HasForeignKey(e => e.IdCursoOfertado);
        });

        modelBuilder.Entity<InscripcionCurso>(entity =>
        {
            entity.ToTable("InscripcionCurso", Esquemas.Acad);
            entity.HasKey(e => e.IdInscripcion);
            entity.Property(e => e.FechaInicio).HasColumnType("date");
            entity.Property(e => e.FechaEstado).HasColumnType("datetime2(0)");
            entity.Property(e => e.FechaRetiro).HasColumnType("date");
            entity.Property(e => e.Observacion).HasMaxLength(500);
            entity.HasOne(e => e.Estudiante)
                .WithMany(s => s.Inscripciones)
                .HasForeignKey(e => e.IdEstudiante);
            entity.HasOne(e => e.CursoOfertado)
                .WithMany(c => c.Inscripciones)
                .HasForeignKey(e => e.IdCursoOfertado);
            entity.HasOne(e => e.EstadoInscripcion)
                .WithMany(s => s.Inscripciones)
                .HasForeignKey(e => e.IdEstadoInscripcion);
        });

        modelBuilder.Entity<Clase>(entity =>
        {
            entity.ToTable("Clase", Esquemas.Acad);
            entity.HasKey(e => e.IdClase);
            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Tema).HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.HasOne(e => e.CursoOfertado)
                .WithMany(c => c.Clases)
                .HasForeignKey(e => e.IdCursoOfertado);
            entity.HasOne(e => e.Horario)
                .WithMany(h => h.Clases)
                .HasForeignKey(e => e.IdHorario);
        });

        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.ToTable("Asistencia", Esquemas.Acad);
            entity.HasKey(e => e.IdAsistencia);
            entity.Property(e => e.Observacion).HasMaxLength(500);
            entity.HasOne(e => e.Clase)
                .WithMany(c => c.Asistencias)
                .HasForeignKey(e => e.IdClase);
            entity.HasOne(e => e.Inscripcion)
                .WithMany(i => i.Asistencias)
                .HasForeignKey(e => e.IdInscripcion);
            entity.HasOne(e => e.EstadoAsistencia)
                .WithMany(s => s.Asistencias)
                .HasForeignKey(e => e.IdEstadoAsistencia);
        });
    }

    private static void ConfigurarCatalogos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoInscripcion>(entity =>
        {
            entity.ToTable("EstadoInscripcion", Esquemas.Cat);
            entity.HasKey(e => e.IdEstadoInscripcion);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });

        modelBuilder.Entity<EstadoAsistencia>(entity =>
        {
            entity.ToTable("EstadoAsistencia", Esquemas.Cat);
            entity.HasKey(e => e.IdEstadoAsistencia);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });

        modelBuilder.Entity<TipoEvaluacion>(entity =>
        {
            entity.ToTable("TipoEvaluacion", Esquemas.Cat);
            entity.HasKey(e => e.IdTipoEvaluacion);
            entity.Property(e => e.Nombre).HasMaxLength(80).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(200);
        });
    }

    private static void ConfigurarEvaluacion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parcial>(entity =>
        {
            entity.ToTable("Parcial", Esquemas.Eval);
            entity.HasKey(e => e.IdParcial);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PuntajeMaximo).HasPrecision(5, 2);
            entity.HasOne(e => e.CursoOfertado)
                .WithMany(c => c.Parciales)
                .HasForeignKey(e => e.IdCursoOfertado);
        });

        modelBuilder.Entity<Evaluacion>(entity =>
        {
            entity.ToTable("Evaluacion", Esquemas.Eval);
            entity.HasKey(e => e.IdEvaluacion);
            entity.Property(e => e.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.PuntajeMaximo).HasPrecision(5, 2);
            entity.Property(e => e.Semana);
            entity.HasOne(e => e.Parcial)
                .WithMany(p => p.Evaluaciones)
                .HasForeignKey(e => e.IdParcial);
            entity.HasOne(e => e.TipoEvaluacion)
                .WithMany(t => t.Evaluaciones)
                .HasForeignKey(e => e.IdTipoEvaluacion);
        });

        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.ToTable("Calificacion", Esquemas.Eval);
            entity.HasKey(e => e.IdCalificacion);
            entity.Property(e => e.PuntajeObtenido).HasPrecision(5, 2);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime2(0)");
            entity.Property(e => e.Observacion).HasMaxLength(500);
            entity.HasOne(e => e.Evaluacion)
                .WithMany(ev => ev.Calificaciones)
                .HasForeignKey(e => e.IdEvaluacion);
            entity.HasOne(e => e.Inscripcion)
                .WithMany(i => i.Calificaciones)
                .HasForeignKey(e => e.IdInscripcion);
        });
    }
}
