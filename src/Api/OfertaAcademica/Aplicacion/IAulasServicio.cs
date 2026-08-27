namespace GestionAcademica.Api.OfertaAcademica.Aplicacion;

public interface IAulasServicio
{
    Task<IReadOnlyList<AulaDto>> ListarAsync(CancellationToken cancellationToken);

    Task<AulaDto> ObtenerAsync(int idAula, CancellationToken cancellationToken);

    Task<AulaDto> CrearAsync(AulaEscrituraDto datos, CancellationToken cancellationToken);

    Task<AulaDto> EditarAsync(int idAula, AulaEscrituraDto datos, CancellationToken cancellationToken);
}

public record AulaDto(
    int IdAula,
    string CodigoAula,
    string? Nombre,
    short? Capacidad,
    string? Ubicacion,
    bool Activo);

public record AulaEscrituraDto(
    string CodigoAula,
    string? Nombre,
    short? Capacidad,
    string? Ubicacion,
    bool Activo = true);
