using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.EstructuraAcademica.Api;

[ApiController]
[Route("api/carreras")]
public class CarrerasController : ControllerBase
{
    private readonly ICarrerasServicio _servicio;

    public CarrerasController(ICarrerasServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CarreraDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }

    [HttpGet("{idCarrera:int}")]
    public async Task<ActionResult<CarreraDto>> Obtener(int idCarrera, CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idCarrera, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<CarreraDto>> Crear(
        [FromBody] CarreraEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idCarrera:int}")]
    public async Task<ActionResult<CarreraDto>> Editar(
        int idCarrera,
        [FromBody] CarreraEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idCarrera, datos, cancellationToken));
    }
}
