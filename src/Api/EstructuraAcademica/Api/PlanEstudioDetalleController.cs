using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.EstructuraAcademica.Api;

[ApiController]
[Route("api/planes-estudio-detalle")]
public class PlanEstudioDetalleController : ControllerBase
{
    private readonly IPlanesEstudioServicio _servicio;

    public PlanEstudioDetalleController(IPlanesEstudioServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpPut("{idPlanDetalle:int}")]
    public async Task<ActionResult<IReadOnlyList<PlanDetalleDto>>> Editar(
        int idPlanDetalle,
        [FromBody] PlanDetalleEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsignaturaAsync(idPlanDetalle, datos, cancellationToken));
    }
}
