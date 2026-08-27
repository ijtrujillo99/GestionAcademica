using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.OfertaAcademica.Api;

[ApiController]
[Route("api/periodos-academicos")]
public class PeriodosAcademicosController : ControllerBase
{
    private readonly IPeriodosAcademicosServicio _servicio;

    public PeriodosAcademicosController(IPeriodosAcademicosServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PeriodoAcademicoDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }

    [HttpGet("{idPeriodo:int}")]
    public async Task<ActionResult<PeriodoAcademicoDto>> Obtener(int idPeriodo, CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idPeriodo, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<PeriodoAcademicoDto>> Crear(
        [FromBody] PeriodoAcademicoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idPeriodo:int}")]
    public async Task<ActionResult<PeriodoAcademicoDto>> Editar(
        int idPeriodo,
        [FromBody] PeriodoAcademicoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idPeriodo, datos, cancellationToken));
    }
}
