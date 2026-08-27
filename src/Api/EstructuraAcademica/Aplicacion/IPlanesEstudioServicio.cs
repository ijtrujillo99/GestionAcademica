namespace GestionAcademica.Api.EstructuraAcademica.Aplicacion;

public interface IPlanesEstudioServicio
{
    Task<IReadOnlyList<PlanEstudioDto>> ListarPorCarreraAsync(int idCarrera, CancellationToken cancellationToken);

    Task<PlanEstudioDto> ObtenerAsync(int idPlanEstudio, CancellationToken cancellationToken);

    Task<PlanEstudioDto> CrearAsync(int idCarrera, PlanEstudioEscrituraDto datos, CancellationToken cancellationToken);

    Task<PlanEstudioDto> EditarAsync(int idPlanEstudio, PlanEstudioEscrituraDto datos, CancellationToken cancellationToken);

    Task<IReadOnlyList<PlanDetalleDto>> ListarAsignaturasAsync(int idPlanEstudio, CancellationToken cancellationToken);

    Task<IReadOnlyList<PlanDetalleDto>> AgregarAsignaturaAsync(
        int idPlanEstudio,
        PlanDetalleEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PlanDetalleDto>> EditarAsignaturaAsync(
        int idPlanDetalle,
        PlanDetalleEscrituraDto datos,
        CancellationToken cancellationToken);
}

public record PlanEstudioDto(
    int IdPlanEstudio,
    int IdCarrera,
    string Nombre,
    short AnioInicio,
    bool Activo);

public record PlanEstudioEscrituraDto(
    string Nombre,
    short AnioInicio,
    bool Activo = true);

public record PlanDetalleDto(
    int IdPlanDetalle,
    int IdPlanEstudio,
    int IdAsignatura,
    string Codigo,
    string Asignatura,
    byte SemestrePlan,
    byte Creditos,
    byte? HorasTeoria,
    byte? HorasPractica,
    short? Orden,
    bool EsElectiva);

public record PlanDetalleEscrituraDto(
    int IdAsignatura,
    byte SemestrePlan,
    byte Creditos,
    byte? HorasTeoria,
    byte? HorasPractica,
    short? Orden,
    bool EsElectiva);
