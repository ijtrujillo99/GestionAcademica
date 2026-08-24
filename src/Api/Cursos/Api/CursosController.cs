using GestionAcademica.Api.Cursos.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Cursos.Api;

[ApiController]
[Route("api/cursos-ofertados")]
public class CursosController : ControllerBase
{
    private readonly ICursosServicio _servicio;

    public CursosController(ICursosServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CursoOfertadoResumenDto>>> Listar(
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }
}
