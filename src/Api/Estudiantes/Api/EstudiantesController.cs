using GestionAcademica.Api.Comun.Paginacion;
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
    [ProducesResponseType(typeof(PagedResult<EstudianteResumenDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<EstudianteResumenDto>>> Listar(
        [FromQuery] int page = PaginacionConsulta.PaginaPredeterminada,
        [FromQuery] int pageSize = PaginacionConsulta.TamanoPredeterminado,
        [FromQuery] string? q = null,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _servicio.ListarAsync(q, page, pageSize, cancellationToken));
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
