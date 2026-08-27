namespace GestionAcademica.Api.Inscripciones.Aplicacion;

public interface IInscripcionesServicio
{
    Task<CursoInscripcionesDto> ListarAsync(int idCursoOfertado, CancellationToken cancellationToken);

    Task<CursoInscripcionesDto> CrearAsync(
        int idCursoOfertado,
        InscripcionEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<CursoInscripcionesDto> EditarAsync(
        int idInscripcion,
        InscripcionEdicionDto datos,
        CancellationToken cancellationToken);
}

public record EstadoInscripcionDto(int IdEstadoInscripcion, string Nombre);

public record InscripcionFilaDto(
    int IdInscripcion,
    int IdEstudiante,
    string Carnet,
    string Nombre,
    string Sexo,
    int IdEstadoInscripcion,
    string Estado,
    bool EsOficial,
    DateTime FechaInicio,
    DateTime? FechaRetiro,
    string? Observacion);

public record CursoInscripcionesDto(
    int IdCursoOfertado,
    IReadOnlyList<EstadoInscripcionDto> Estados,
    IReadOnlyList<InscripcionFilaDto> Filas);

public record InscripcionEscrituraDto(
    int IdEstudiante,
    bool? EsOficial,
    int? IdEstadoInscripcion,
    string? Observacion);

public record InscripcionEdicionDto(
    int IdEstadoInscripcion,
    bool EsOficial,
    string? Observacion);
