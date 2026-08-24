using GestionAcademica.Api.Comun.Errores;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Salud.Api;

[ApiController]
[Route("api/salud")]
public class SaludController : ControllerBase
{
    private readonly GestionAcademicaDbContext _contexto;

    public SaludController(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    [HttpGet]
    public async Task<ActionResult<SaludDto>> Verificar(CancellationToken cancellationToken)
    {
        var conectada = await _contexto.Database.CanConnectAsync(cancellationToken);
        if (!conectada)
        {
            return Problem(
                type: ErrorHttp.Tipo(StatusCodes.Status503ServiceUnavailable),
                title: "Servicio no disponible",
                detail: "No se pudo conectar a SQL Server. Verifica que el servicio MSSQLSERVER esté en ejecución.",
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }

        var estudiantes = await _contexto.Estudiantes.CountAsync(cancellationToken);
        var servidor = _contexto.Database.GetDbConnection().DataSource;

        return Ok(new SaludDto(true, "API y base GestionAcademica disponibles.", servidor, estudiantes));
    }
}

public record SaludDto(bool Conectada, string Mensaje, string? Servidor, int Estudiantes);
