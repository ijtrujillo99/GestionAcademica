using GestionAcademica.Api.ModuloAsistencia.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.ModuloAsistencia.Api;

[ApiController]
[Route("api/cursos-ofertados/{idCursoOfertado:int}/clases")]
public class AsistenciaController : ControllerBase
{
    private readonly IAsistenciaServicio _servicio;

    public AsistenciaController(IAsistenciaServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClaseResumenDto>>> Listar(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarClasesAsync(idCursoOfertado, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<ClaseResumenDto>> Crear(
        int idCursoOfertado,
        [FromBody] ClaseEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearClaseAsync(idCursoOfertado, datos, cancellationToken));
    }
}
