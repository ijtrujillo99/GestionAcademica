using GestionAcademica.Api.Academico.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Academico.Api;

[ApiController]
[Route("api/academico")]
public class AcademicoController : ControllerBase
{
    private readonly IAcademicoServicio _servicio;

    public AcademicoController(IAcademicoServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<AcademicoResumenDto>> Obtener(CancellationToken cancellationToken)
    {
        return Ok(await _servicio.ObtenerResumenAsync(cancellationToken));
    }
}
