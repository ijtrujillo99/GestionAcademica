using GestionAcademica.Api.ModuloEvaluacion.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.ModuloEvaluacion.Api;

[ApiController]
[Route("api/evaluaciones")]
public class EvaluacionEdicionController : ControllerBase
{
    private readonly IEvaluacionServicio _servicio;

    public EvaluacionEdicionController(IEvaluacionServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EvaluacionIndividualDto>>> Listar(
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarTodasAsync(cancellationToken));
    }

    [HttpGet("{idEvaluacion:int}")]
    public async Task<ActionResult<EvaluacionIndividualDto>> Obtener(
        int idEvaluacion,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerEvaluacionAsync(idEvaluacion, cancellationToken));
    }

    [HttpPut("{idEvaluacion:int}")]
    public async Task<ActionResult<ParcialEvaluacionesDto>> Editar(
        int idEvaluacion,
        [FromBody] EvaluacionEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarEvaluacionAsync(idEvaluacion, datos, cancellationToken));
    }
}
