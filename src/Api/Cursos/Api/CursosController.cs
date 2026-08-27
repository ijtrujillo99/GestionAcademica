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

    [HttpGet("{idCursoOfertado:int}")]
    public async Task<ActionResult<CursoOfertadoDetalleDto>> Obtener(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idCursoOfertado, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<CursoOfertadoDetalleDto>> Crear(
        [FromBody] CursoOfertadoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idCursoOfertado:int}")]
    public async Task<ActionResult<CursoOfertadoDetalleDto>> Editar(
        int idCursoOfertado,
        [FromBody] CursoOfertadoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idCursoOfertado, datos, cancellationToken));
    }
}
