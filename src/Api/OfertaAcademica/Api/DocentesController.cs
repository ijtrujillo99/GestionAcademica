using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.OfertaAcademica.Api;

[ApiController]
[Route("api/docentes")]
public class DocentesController : ControllerBase
{
    private readonly IDocentesServicio _servicio;

    public DocentesController(IDocentesServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DocenteDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }

    [HttpGet("{idDocente:int}")]
    public async Task<ActionResult<DocenteDto>> Obtener(int idDocente, CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idDocente, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<DocenteDto>> Crear(
        [FromBody] DocenteEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idDocente:int}")]
    public async Task<ActionResult<DocenteDto>> Editar(
        int idDocente,
        [FromBody] DocenteEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idDocente, datos, cancellationToken));
    }
}
