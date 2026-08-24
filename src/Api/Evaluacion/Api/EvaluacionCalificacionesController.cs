using GestionAcademica.Api.ModuloEvaluacion.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.ModuloEvaluacion.Api;

[ApiController]
[Route("api/evaluaciones/{idEvaluacion:int}/calificaciones")]
public class EvaluacionCalificacionesController : ControllerBase
{
    private readonly IEvaluacionServicio _servicio;

    public EvaluacionCalificacionesController(IEvaluacionServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<EvaluacionCalificacionesDto>> Obtener(
        int idEvaluacion,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerCalificacionesAsync(idEvaluacion, cancellationToken));
    }

    [HttpPut]
    public async Task<ActionResult<EvaluacionCalificacionesDto>> Guardar(
        int idEvaluacion,
        [FromBody] IReadOnlyList<CalificacionCambioDto> cambios,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.GuardarCalificacionesAsync(
            idEvaluacion,
            cambios ?? Array.Empty<CalificacionCambioDto>(),
            cancellationToken));
    }
}
