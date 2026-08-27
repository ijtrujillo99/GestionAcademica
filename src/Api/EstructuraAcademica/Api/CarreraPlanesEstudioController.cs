using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.EstructuraAcademica.Api;

[ApiController]
[Route("api/carreras/{idCarrera:int}/planes-estudio")]
public class CarreraPlanesEstudioController : ControllerBase
{
    private readonly IPlanesEstudioServicio _servicio;

    public CarreraPlanesEstudioController(IPlanesEstudioServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PlanEstudioDto>>> Listar(
        int idCarrera,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarPorCarreraAsync(idCarrera, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<PlanEstudioDto>> Crear(
        int idCarrera,
        [FromBody] PlanEstudioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(idCarrera, datos, cancellationToken));
    }
}
