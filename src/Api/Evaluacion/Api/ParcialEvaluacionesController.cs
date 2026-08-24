using GestionAcademica.Api.ModuloEvaluacion.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.ModuloEvaluacion.Api;

[ApiController]
[Route("api/parciales/{idParcial:int}/evaluaciones")]
public class ParcialEvaluacionesController : ControllerBase
{
    private readonly IEvaluacionServicio _servicio;

    public ParcialEvaluacionesController(IEvaluacionServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<ParcialEvaluacionesDto>> Listar(
        int idParcial,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarEvaluacionesAsync(idParcial, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<ParcialEvaluacionesDto>> Crear(
        int idParcial,
        [FromBody] EvaluacionEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearEvaluacionAsync(idParcial, datos, cancellationToken));
    }
}
