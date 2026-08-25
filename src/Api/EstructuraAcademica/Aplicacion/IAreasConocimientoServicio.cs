namespace GestionAcademica.Api.EstructuraAcademica.Aplicacion;

public interface IAreasConocimientoServicio
{
    Task<IReadOnlyList<AreaConocimientoDto>> ListarAsync(CancellationToken cancellationToken);

    Task<AreaConocimientoDto> ObtenerAsync(int idAreaConocimiento, CancellationToken cancellationToken);

    Task<AreaConocimientoDto> CrearAsync(AreaConocimientoEscrituraDto datos, CancellationToken cancellationToken);

    Task<AreaConocimientoDto> EditarAsync(
        int idAreaConocimiento,
        AreaConocimientoEscrituraDto datos,
        CancellationToken cancellationToken);
}

public record AreaConocimientoDto(
    int IdAreaConocimiento,
    string Nombre,
    string? Descripcion,
    bool Activo);

public record AreaConocimientoEscrituraDto(
    string Nombre,
    string? Descripcion,
    bool Activo = true);
