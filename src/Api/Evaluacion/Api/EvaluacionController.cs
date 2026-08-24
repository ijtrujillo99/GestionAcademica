using GestionAcademica.Api.ModuloEvaluacion.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.ModuloEvaluacion.Api;

[ApiController]
[Route("api/cursos-ofertados/{idCursoOfertado:int}/parciales")]
public class EvaluacionController : ControllerBase
{
    private readonly IEvaluacionServicio _servicio;

    public EvaluacionController(IEvaluacionServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ParcialResumenDto>>> Listar(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarParcialesAsync(idCursoOfertado, cancellationToken));
    }
}
