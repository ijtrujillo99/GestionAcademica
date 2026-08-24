namespace GestionAcademica.Api.ModuloEvaluacion.Aplicacion;

public interface IEvaluacionServicio
{
    Task<IReadOnlyList<ParcialResumenDto>> ListarParcialesAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken);

    Task<ParcialEvaluacionesDto> ListarEvaluacionesAsync(int idParcial, CancellationToken cancellationToken);

    Task<EvaluacionIndividualDto> ObtenerEvaluacionAsync(int idEvaluacion, CancellationToken cancellationToken);

    Task<IReadOnlyList<EvaluacionIndividualDto>> ListarTodasAsync(CancellationToken cancellationToken);

    Task<ParcialEvaluacionesDto> CrearEvaluacionAsync(
        int idParcial,
        EvaluacionEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<ParcialEvaluacionesDto> EditarEvaluacionAsync(
        int idEvaluacion,
        EvaluacionEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<EvaluacionCalificacionesDto> ObtenerCalificacionesAsync(
        int idEvaluacion,
        CancellationToken cancellationToken);

    Task<EvaluacionCalificacionesDto> GuardarCalificacionesAsync(
        int idEvaluacion,
        IReadOnlyList<CalificacionCambioDto> cambios,
        CancellationToken cancellationToken);
}

public record ParcialResumenDto(
    int IdParcial,
    string Nombre,
    byte Orden,
    decimal PuntajeMaximo,
    int Evaluaciones);

public record EvaluacionDetalleDto(
    int IdEvaluacion,
    int IdTipoEvaluacion,
    string Nombre,
    string Tipo,
    string? Descripcion,
    byte? Semana,
    decimal PuntajeMaximo,
    bool EsObligatoria,
    byte Orden);

public record EvaluacionIndividualDto(
    int IdEvaluacion,
    int IdParcial,
    int IdTipoEvaluacion,
    string Nombre,
    string Tipo,
    string? Descripcion,
    byte? Semana,
    decimal PuntajeMaximo,
    bool EsObligatoria,
    byte Orden);

public record ParcialEvaluacionesDto(
    int IdParcial,
    string Nombre,
    decimal PuntajeMaximo,
    decimal SumaPuntajes,
    IReadOnlyList<EvaluacionDetalleDto> Evaluaciones);

public record EvaluacionEscrituraDto(
    int IdTipoEvaluacion,
    string Nombre,
    string? Descripcion,
    decimal PuntajeMaximo,
    bool EsObligatoria,
    byte Orden,
    byte? Semana);

public record CalificacionFilaDto(
    int IdInscripcion,
    int IdEstudiante,
    string Carnet,
    string Nombre,
    bool EsOficial,
    string EstadoInscripcion,
    decimal? PuntajeObtenido,
    string? Observacion);

public record EvaluacionCalificacionesDto(
    int IdEvaluacion,
    int IdParcial,
    int IdCursoOfertado,
    string Nombre,
    decimal PuntajeMaximo,
    bool EsObligatoria,
    IReadOnlyList<CalificacionFilaDto> Filas);

public record CalificacionCambioDto(
    int IdInscripcion,
    decimal PuntajeObtenido,
    string? Observacion);
