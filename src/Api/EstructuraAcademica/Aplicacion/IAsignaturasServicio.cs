namespace GestionAcademica.Api.EstructuraAcademica.Aplicacion;

public interface IAsignaturasServicio
{
    Task<IReadOnlyList<AsignaturaDto>> ListarAsync(CancellationToken cancellationToken);

    Task<AsignaturaDto> ObtenerAsync(int idAsignatura, CancellationToken cancellationToken);

    Task<AsignaturaDto> CrearAsync(AsignaturaEscrituraDto datos, CancellationToken cancellationToken);

    Task<AsignaturaDto> EditarAsync(int idAsignatura, AsignaturaEscrituraDto datos, CancellationToken cancellationToken);
}

public record AsignaturaDto(
    int IdAsignatura,
    string Codigo,
    string Nombre,
    string? Descripcion,
    bool Activo);

public record AsignaturaEscrituraDto(
    string Codigo,
    string Nombre,
    string? Descripcion,
    bool Activo = true);
