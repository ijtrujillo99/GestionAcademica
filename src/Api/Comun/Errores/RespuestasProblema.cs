using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Comun.Errores;

public static class RespuestasProblema
{
    public const string ContentType = "application/problem+json";

    public static IActionResult Validacion(ActionContext contexto)
    {
        var problema = new ValidationProblemDetails(contexto.ModelState)
        {
            Type = ErrorHttp.Tipo(StatusCodes.Status400BadRequest),
            Title = "Datos inválidos",
            Status = StatusCodes.Status400BadRequest,
            Detail = "Hay errores de validación en la solicitud.",
            Instance = contexto.HttpContext.Request.Path
        };
        problema.Extensions["traceId"] = contexto.HttpContext.TraceIdentifier;

        return new BadRequestObjectResult(problema)
        {
            ContentTypes = { ContentType }
        };
    }
}
