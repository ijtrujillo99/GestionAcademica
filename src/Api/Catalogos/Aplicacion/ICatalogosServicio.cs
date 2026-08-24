namespace GestionAcademica.Api.Catalogos.Aplicacion;

public interface ICatalogosServicio
{
    Task<CatalogosResumenDto> ObtenerResumenAsync(CancellationToken cancellationToken);
}

public record CatalogosResumenDto(
    int EstadosInscripcion,
    int EstadosAsistencia,
    int TiposEvaluacion);
