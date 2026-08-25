using GestionAcademica.Api.Cursos.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Cursos.Api;

[ApiController]
[Route("api/cursos-ofertados/{idCursoOfertado:int}/horarios")]
public class CursoHorariosController : ControllerBase
{
    private readonly ICursosServicio _servicio;

    public CursoHorariosController(ICursosServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HorarioCursoDto>>> Listar(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarHorariosAsync(idCursoOfertado, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<IReadOnlyList<HorarioCursoDto>>> Crear(
        int idCursoOfertado,
        [FromBody] HorarioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearHorarioAsync(idCursoOfertado, datos, cancellationToken));
    }
}
