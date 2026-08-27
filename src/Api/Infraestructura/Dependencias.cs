using GestionAcademica.Api.Academico.Aplicacion;
using GestionAcademica.Api.Academico.Datos;
using GestionAcademica.Api.ModuloAsistencia.Aplicacion;
using GestionAcademica.Api.ModuloAsistencia.Datos;
using GestionAcademica.Api.Catalogos.Aplicacion;
using GestionAcademica.Api.Catalogos.Datos;
using GestionAcademica.Api.Cursos.Aplicacion;
using GestionAcademica.Api.Cursos.Datos;
using GestionAcademica.Api.Estudiantes.Aplicacion;
using GestionAcademica.Api.Estudiantes.Datos;
using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using GestionAcademica.Api.EstructuraAcademica.Datos;
using GestionAcademica.Api.Inscripciones.Aplicacion;
using GestionAcademica.Api.Inscripciones.Datos;
using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using GestionAcademica.Api.OfertaAcademica.Datos;
using GestionAcademica.Api.ModuloEvaluacion.Aplicacion;
using GestionAcademica.Api.ModuloEvaluacion.Datos;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.Infraestructura.Reportes;
using GestionAcademica.Api.Reportes.Aplicacion;
using GestionAcademica.Api.Reportes.Datos;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Infraestructura;

public static class Dependencias
{
    public static IServiceCollection AddGestionAcademica(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var conexion = configuration.GetConnectionString("GestionAcademica")
            ?? throw new InvalidOperationException(
                "Falta ConnectionStrings:GestionAcademica en appsettings.");

        services.AddDbContext<GestionAcademicaDbContext>(options =>
            options.UseSqlServer(conexion));

        services.AddScoped<ICatalogosServicio, CatalogosServicio>();
        services.AddScoped<IAcademicoServicio, AcademicoServicio>();
        services.AddScoped<ICursosServicio, CursosServicio>();
        services.AddScoped<IEstudiantesServicio, EstudiantesServicio>();
        services.AddScoped<IInscripcionesServicio, InscripcionesServicio>();
        services.AddScoped<IAreasConocimientoServicio, AreasConocimientoServicio>();
        services.AddScoped<ICarrerasServicio, CarrerasServicio>();
        services.AddScoped<IPlanesEstudioServicio, PlanesEstudioServicio>();
        services.AddScoped<IAsignaturasServicio, AsignaturasServicio>();
        services.AddScoped<IPeriodosAcademicosServicio, PeriodosAcademicosServicio>();
        services.AddScoped<IGruposServicio, GruposServicio>();
        services.AddScoped<IAulasServicio, AulasServicio>();
        services.AddScoped<IDocentesServicio, DocentesServicio>();
        services.AddScoped<IAsistenciaServicio, AsistenciaServicio>();
        services.AddScoped<IEvaluacionServicio, EvaluacionServicio>();
        services.AddScoped<IEjecutorProcedimientos, EjecutorProcedimientos>();
        services.AddScoped<IReportesServicio, ReportesServicio>();

        return services;
    }
}
