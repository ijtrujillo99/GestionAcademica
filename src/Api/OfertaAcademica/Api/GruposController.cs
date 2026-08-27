using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.OfertaAcademica.Api;

[ApiController]
[Route("api/grupos")]
public class GruposController : ControllerBase
{
    private readonly IGruposServicio _servicio;

    public GruposController(IGruposServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GrupoDto>>> Listar(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ListarAsync(cancellationToken));
    }

    [HttpGet("{idGrupo:int}")]
    public async Task<ActionResult<GrupoDto>> Obtener(int idGrupo, CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerAsync(idGrupo, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<GrupoDto>> Crear(
        [FromBody] GrupoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CrearAsync(datos, cancellationToken));
    }

    [HttpPut("{idGrupo:int}")]
    public async Task<ActionResult<GrupoDto>> Editar(
        int idGrupo,
        [FromBody] GrupoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarAsync(idGrupo, datos, cancellationToken));
    }
}
