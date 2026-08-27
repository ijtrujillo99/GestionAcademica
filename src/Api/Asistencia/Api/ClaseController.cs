using GestionAcademica.Api.ModuloAsistencia.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.ModuloAsistencia.Api;

[ApiController]
[Route("api/clases")]
public class ClaseController : ControllerBase
{
    private readonly IAsistenciaServicio _servicio;

    public ClaseController(IAsistenciaServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpPut("{idClase:int}")]
    public async Task<ActionResult<ClaseResumenDto>> Editar(
        int idClase,
        [FromBody] ClaseEdicionDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarClaseAsync(idClase, datos, cancellationToken));
    }
}
