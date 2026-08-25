using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.EstructuraAcademica.Api;

[ApiController]
[Route("api/areas-conocimiento")]
public class AreasConocimientoController : ControllerBase
{
    private readonly IAreasConocimientoServicio _servicio;

    public AreasConocimientoController(IAreasConocimientoServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AreaConocimientoDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }

    [HttpGet("{idAreaConocimiento:int}")]
    public async Task<ActionResult<AreaConocimientoDto>> Obtener(
        int idAreaConocimiento,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idAreaConocimiento, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<AreaConocimientoDto>> Crear(
        [FromBody] AreaConocimientoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idAreaConocimiento:int}")]
    public async Task<ActionResult<AreaConocimientoDto>> Editar(
        int idAreaConocimiento,
        [FromBody] AreaConocimientoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idAreaConocimiento, datos, cancellationToken));
    }
}
