using GestionAcademica.Api.Infraestructura.Reportes;
using GestionAcademica.Api.Reportes.Aplicacion;
using Microsoft.Data.SqlClient;

namespace GestionAcademica.Api.Reportes.Datos;

public class ReportesServicio : IReportesServicio
{
    private readonly IEjecutorProcedimientos _ejecutor;

    public ReportesServicio(IEjecutorProcedimientos ejecutor)
    {
        _ejecutor = ejecutor;
    }

    public async Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> AsistenciaEstudianteAsync(
        int idCursoOfertado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        var conjuntos = await _ejecutor.EjecutarAsync(
            NombresProcedimientos.AsistenciaEstudiante,
            ParametrosCursoYFechas(idCursoOfertado, fechaDesde, fechaHasta),
            cancellationToken);

        return PrimerConjunto(conjuntos);
    }

    public async Task<ReporteAsistenciaSesionDto> AsistenciaSesionAsync(
        int idCursoOfertado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        var conjuntos = await _ejecutor.EjecutarAsync(
            NombresProcedimientos.AsistenciaSesion,
            ParametrosCursoYFechas(idCursoOfertado, fechaDesde, fechaHasta),
            cancellationToken);

        return new ReporteAsistenciaSesionDto(
            conjuntos.ElementAtOrDefault(0) ?? Array.Empty<IReadOnlyDictionary<string, object?>>(),
            conjuntos.ElementAtOrDefault(1) ?? Array.Empty<IReadOnlyDictionary<string, object?>>());
    }

    public async Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> AsistenciaSemanalAsync(
        int idCursoOfertado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        var conjuntos = await _ejecutor.EjecutarAsync(
            NombresProcedimientos.AsistenciaSemanal,
            ParametrosCursoYFechas(idCursoOfertado, fechaDesde, fechaHasta),
            cancellationToken);

        return PrimerConjunto(conjuntos);
    }

    public async Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> CalificacionesAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        var conjuntos = await _ejecutor.EjecutarAsync(
            NombresProcedimientos.Calificaciones,
            new[] { new SqlParameter("@IdCursoOfertado", idCursoOfertado) },
            cancellationToken);

        return PrimerConjunto(conjuntos);
    }

    public async Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> SituacionAcademicaAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        var conjuntos = await _ejecutor.EjecutarAsync(
            NombresProcedimientos.SituacionAcademica,
            new[] { new SqlParameter("@IdCursoOfertado", idCursoOfertado) },
            cancellationToken);

        return PrimerConjunto(conjuntos);
    }

    private static IReadOnlyList<IReadOnlyDictionary<string, object?>> PrimerConjunto(
        IReadOnlyList<IReadOnlyList<IReadOnlyDictionary<string, object?>>> conjuntos)
    {
        return conjuntos.Count == 0
            ? Array.Empty<IReadOnlyDictionary<string, object?>>()
            : conjuntos[0];
    }

    private static IReadOnlyList<SqlParameter> ParametrosCursoYFechas(
        int idCursoOfertado,
        DateTime? fechaDesde,
        DateTime? fechaHasta)
    {
        return new[]
        {
            new SqlParameter("@IdCursoOfertado", idCursoOfertado),
            Fecha("@FechaDesde", fechaDesde),
            Fecha("@FechaHasta", fechaHasta)
        };
    }

    private static SqlParameter Fecha(string nombre, DateTime? valor)
    {
        return new SqlParameter(nombre, System.Data.SqlDbType.Date)
        {
            Value = valor.HasValue ? valor.Value.Date : DBNull.Value
        };
    }
}
