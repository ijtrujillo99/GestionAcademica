namespace GestionAcademica.Api.ModuloAsistencia.Aplicacion;

public interface IAsistenciaServicio
{
    Task<IReadOnlyList<ClaseResumenDto>> ListarClasesAsync(int idCursoOfertado, CancellationToken cancellationToken);

    Task<ClaseAsistenciaDto> ObtenerListaAsync(int idClase, CancellationToken cancellationToken);

    Task<ClaseAsistenciaDto> GuardarAsync(
        int idClase,
        IReadOnlyList<AsistenciaCambioDto> cambios,
        CancellationToken cancellationToken);
}

public record ClaseResumenDto(
    int IdClase,
    DateTime Fecha,
    string? Tema,
    int AsistenciasRegistradas);

public record EstadoAsistenciaDto(int IdEstadoAsistencia, string Nombre);

public record AsistenciaFilaDto(
    int IdInscripcion,
    int IdEstudiante,
    string Carnet,
    string Nombre,
    string Sexo,
    bool EsOficial,
    string EstadoInscripcion,
    int? IdEstadoAsistencia,
    string? EstadoAsistencia,
    string? Observacion);

public record ClaseAsistenciaDto(
    int IdClase,
    int IdCursoOfertado,
    DateTime Fecha,
    string? Tema,
    IReadOnlyList<EstadoAsistenciaDto> Estados,
    IReadOnlyList<AsistenciaFilaDto> Filas);

public record AsistenciaCambioDto(
    int IdInscripcion,
    int IdEstadoAsistencia,
    string? Observacion);
