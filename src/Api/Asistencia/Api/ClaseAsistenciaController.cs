using GestionAcademica.Api.ModuloAsistencia.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.ModuloAsistencia.Api;

[ApiController]
[Route("api/clases/{idClase:int}/asistencias")]
public class ClaseAsistenciaController : ControllerBase
{
    private readonly IAsistenciaServicio _servicio;

    public ClaseAsistenciaController(IAsistenciaServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<ClaseAsistenciaDto>> Obtener(
        int idClase,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerListaAsync(idClase, cancellationToken));
    }

    [HttpPut]
    public async Task<ActionResult<ClaseAsistenciaDto>> Guardar(
        int idClase,
        [FromBody] IReadOnlyList<AsistenciaCambioDto> cambios,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.GuardarAsync(
            idClase,
            cambios ?? Array.Empty<AsistenciaCambioDto>(),
            cancellationToken));
    }
}
