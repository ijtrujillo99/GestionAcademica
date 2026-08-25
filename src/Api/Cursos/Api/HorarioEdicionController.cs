using GestionAcademica.Api.Cursos.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Cursos.Api;

[ApiController]
[Route("api/horarios")]
public class HorarioEdicionController : ControllerBase
{
    private readonly ICursosServicio _servicio;

    public HorarioEdicionController(ICursosServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpPut("{idHorario:int}")]
    public async Task<ActionResult<HorarioCursoDto>> Editar(
        int idHorario,
        [FromBody] HorarioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.EditarHorarioAsync(idHorario, datos, cancellationToken));
    }
}
