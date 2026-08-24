using GestionAcademica.Api.Inscripciones.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Inscripciones.Api;

[ApiController]
[Route("api/cursos-ofertados/{idCursoOfertado:int}/inscripciones")]
public class CursoInscripcionesController : ControllerBase
{
    private readonly IInscripcionesServicio _servicio;

    public CursoInscripcionesController(IInscripcionesServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<CursoInscripcionesDto>> Listar(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(idCursoOfertado, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<CursoInscripcionesDto>> Crear(
        int idCursoOfertado,
        [FromBody] InscripcionEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(idCursoOfertado, datos, cancellationToken));
    }
}
