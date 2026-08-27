namespace GestionAcademica.Api.OfertaAcademica.Aplicacion;

public interface IDocentesServicio
{
    Task<IReadOnlyList<DocenteDto>> ListarAsync(CancellationToken cancellationToken);

    Task<DocenteDto> ObtenerAsync(int idDocente, CancellationToken cancellationToken);

    Task<DocenteDto> CrearAsync(DocenteEscrituraDto datos, CancellationToken cancellationToken);

    Task<DocenteDto> EditarAsync(int idDocente, DocenteEscrituraDto datos, CancellationToken cancellationToken);
}

public record DocenteDto(
    int IdDocente,
    string Nombre,
    string Correo,
    string? Telefono,
    bool Activo);

public record DocenteEscrituraDto(
    string Nombre,
    string Correo,
    string? Telefono,
    bool Activo = true);
