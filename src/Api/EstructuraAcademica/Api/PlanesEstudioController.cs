using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.EstructuraAcademica.Api;

[ApiController]
[Route("api/planes-estudio")]
public class PlanesEstudioController : ControllerBase
{
    private readonly IPlanesEstudioServicio _servicio;

    public PlanesEstudioController(IPlanesEstudioServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet("{idPlanEstudio:int}")]
    public async Task<ActionResult<PlanEstudioDto>> Obtener(
        int idPlanEstudio,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idPlanEstudio, cancellationToken));
    }

    [HttpPut("{idPlanEstudio:int}")]
    public async Task<ActionResult<PlanEstudioDto>> Editar(
        int idPlanEstudio,
        [FromBody] PlanEstudioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idPlanEstudio, datos, cancellationToken));
    }

    [HttpGet("{idPlanEstudio:int}/asignaturas")]
    public async Task<ActionResult<IReadOnlyList<PlanDetalleDto>>> ListarAsignaturas(
        int idPlanEstudio,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsignaturasAsync(idPlanEstudio, cancellationToken));
    }

    [HttpPost("{idPlanEstudio:int}/asignaturas")]
    public async Task<ActionResult<IReadOnlyList<PlanDetalleDto>>> AgregarAsignatura(
        int idPlanEstudio,
        [FromBody] PlanDetalleEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.AgregarAsignaturaAsync(idPlanEstudio, datos, cancellationToken));
    }
}
