namespace GestionAcademica.Api.EstructuraAcademica.Aplicacion;

public interface ICarrerasServicio
{
    Task<IReadOnlyList<CarreraDto>> ListarAsync(CancellationToken cancellationToken);

    Task<CarreraDto> ObtenerAsync(int idCarrera, CancellationToken cancellationToken);

    Task<CarreraDto> CrearAsync(CarreraEscrituraDto datos, CancellationToken cancellationToken);

    Task<CarreraDto> EditarAsync(int idCarrera, CarreraEscrituraDto datos, CancellationToken cancellationToken);
}

public record CarreraDto(
    int IdCarrera,
    int IdAreaConocimiento,
    string Area,
    string Nombre,
    string? Descripcion,
    bool Activo);

public record CarreraEscrituraDto(
    int IdAreaConocimiento,
    string Nombre,
    string? Descripcion,
    bool Activo = true);
