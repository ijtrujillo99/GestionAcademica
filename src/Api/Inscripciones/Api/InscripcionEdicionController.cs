using GestionAcademica.Api.Inscripciones.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Inscripciones.Api;

[ApiController]
[Route("api/inscripciones")]
public class InscripcionEdicionController : ControllerBase
{
    private readonly IInscripcionesServicio _servicio;

    public InscripcionEdicionController(IInscripcionesServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpPut("{idInscripcion:int}")]
    public async Task<ActionResult<CursoInscripcionesDto>> Editar(
        int idInscripcion,
        [FromBody] InscripcionEdicionDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idInscripcion, datos, cancellationToken));
    }
}
