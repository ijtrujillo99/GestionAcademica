using GestionAcademica.Api.Academico.Aplicacion;
using GestionAcademica.Api.Academico.Datos;
using GestionAcademica.Api.ModuloAsistencia.Aplicacion;
using GestionAcademica.Api.ModuloAsistencia.Datos;
using GestionAcademica.Api.Catalogos.Aplicacion;
using GestionAcademica.Api.Catalogos.Datos;
using GestionAcademica.Api.Cursos.Aplicacion;
using GestionAcademica.Api.Cursos.Datos;
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
        services.AddScoped<IAsistenciaServicio, AsistenciaServicio>();
        services.AddScoped<IEvaluacionServicio, EvaluacionServicio>();
        services.AddScoped<IEjecutorProcedimientos, EjecutorProcedimientos>();
        services.AddScoped<IReportesServicio, ReportesServicio>();

        return services;
    }
}
