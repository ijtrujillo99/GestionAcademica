using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.EstructuraAcademica.Api;

[ApiController]
[Route("api/asignaturas")]
public class AsignaturasController : ControllerBase
{
    private readonly IAsignaturasServicio _servicio;

    public AsignaturasController(IAsignaturasServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AsignaturaDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }

    [HttpGet("{idAsignatura:int}")]
    public async Task<ActionResult<AsignaturaDto>> Obtener(int idAsignatura, CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idAsignatura, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<AsignaturaDto>> Crear(
        [FromBody] AsignaturaEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idAsignatura:int}")]
    public async Task<ActionResult<AsignaturaDto>> Editar(
        int idAsignatura,
        [FromBody] AsignaturaEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idAsignatura, datos, cancellationToken));
    }
}
