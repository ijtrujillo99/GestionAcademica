using GestionAcademica.Api.Catalogos.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Catalogos.Api;

[ApiController]
[Route("api/catalogos")]
public class CatalogosController : ControllerBase
{
    private readonly ICatalogosServicio _servicio;

    public CatalogosController(ICatalogosServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<CatalogosResumenDto>> Obtener(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerResumenAsync(cancellationToken));
    }
}
