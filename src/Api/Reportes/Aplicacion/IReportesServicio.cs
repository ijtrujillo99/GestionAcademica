namespace GestionAcademica.Api.Reportes.Aplicacion;

public interface IReportesServicio
{
    Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> AsistenciaEstudianteAsync(
        int idCursoOfertado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken);

    Task<ReporteAsistenciaSesionDto> AsistenciaSesionAsync(
        int idCursoOfertado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> AsistenciaSemanalAsync(
        int idCursoOfertado,
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> CalificacionesAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<IReadOnlyDictionary<string, object?>>> SituacionAcademicaAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken);
}

public record ReporteAsistenciaSesionDto(
    IReadOnlyList<IReadOnlyDictionary<string, object?>> Sesiones,
    IReadOnlyList<IReadOnlyDictionary<string, object?>> PorSexo);
