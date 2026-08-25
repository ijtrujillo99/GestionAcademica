namespace GestionAcademica.Api.OfertaAcademica.Aplicacion;

public interface IPeriodosAcademicosServicio
{
    Task<IReadOnlyList<PeriodoAcademicoDto>> ListarAsync(CancellationToken cancellationToken);

    Task<PeriodoAcademicoDto> ObtenerAsync(int idPeriodo, CancellationToken cancellationToken);

    Task<PeriodoAcademicoDto> CrearAsync(PeriodoAcademicoEscrituraDto datos, CancellationToken cancellationToken);

    Task<PeriodoAcademicoDto> EditarAsync(
        int idPeriodo,
        PeriodoAcademicoEscrituraDto datos,
        CancellationToken cancellationToken);
}

public record PeriodoAcademicoDto(
    int IdPeriodo,
    short Anio,
    byte Semestre,
    DateTime FechaInicio,
    DateTime FechaFin,
    bool Activo);

public record PeriodoAcademicoEscrituraDto(
    short Anio,
    byte Semestre,
    DateTime FechaInicio,
    DateTime FechaFin,
    bool Activo = true);
