using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.OfertaAcademica.Api;

[ApiController]
[Route("api/aulas")]
public class AulasController : ControllerBase
{
    private readonly IAulasServicio _servicio;

    public AulasController(IAulasServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AulaDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }

    [HttpGet("{idAula:int}")]
    public async Task<ActionResult<AulaDto>> Obtener(int idAula, CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idAula, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<AulaDto>> Crear(
        [FromBody] AulaEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idAula:int}")]
    public async Task<ActionResult<AulaDto>> Editar(
        int idAula,
        [FromBody] AulaEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idAula, datos, cancellationToken));
    }
}
