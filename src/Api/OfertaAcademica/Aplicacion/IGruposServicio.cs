namespace GestionAcademica.Api.OfertaAcademica.Aplicacion;

public interface IGruposServicio
{
    Task<IReadOnlyList<GrupoDto>> ListarAsync(CancellationToken cancellationToken);

    Task<GrupoDto> ObtenerAsync(int idGrupo, CancellationToken cancellationToken);

    Task<GrupoDto> CrearAsync(GrupoEscrituraDto datos, CancellationToken cancellationToken);

    Task<GrupoDto> EditarAsync(int idGrupo, GrupoEscrituraDto datos, CancellationToken cancellationToken);
}

public record GrupoDto(
    int IdGrupo,
    int IdPeriodo,
    int IdCarrera,
    string CodigoGrupo,
    string? Nombre,
    string? Descripcion,
    bool Activo,
    string Carrera,
    short Anio,
    byte Semestre);

public record GrupoEscrituraDto(
    int IdPeriodo,
    int IdCarrera,
    string CodigoGrupo,
    string? Nombre,
    string? Descripcion,
    bool Activo = true);
