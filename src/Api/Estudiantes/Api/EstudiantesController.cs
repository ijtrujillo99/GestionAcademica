using GestionAcademica.Api.Estudiantes.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Estudiantes.Api;

[ApiController]
[Route("api/estudiantes")]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudiantesServicio _servicio;

    public EstudiantesController(IEstudiantesServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EstudianteResumenDto>>> Listar(
        [FromQuery] string? q,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(q, cancellationToken));
    }

    [HttpGet("{idEstudiante:int}")]
    public async Task<ActionResult<EstudianteResumenDto>> Obtener(
        int idEstudiante,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idEstudiante, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<EstudianteResumenDto>> Crear(
        [FromBody] EstudianteEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idEstudiante:int}")]
    public async Task<ActionResult<EstudianteResumenDto>> Editar(
        int idEstudiante,
        [FromBody] EstudianteEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idEstudiante, datos, cancellationToken));
    }
}
