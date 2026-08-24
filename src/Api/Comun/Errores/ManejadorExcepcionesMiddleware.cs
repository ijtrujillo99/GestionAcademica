using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Comun.Errores;

public sealed class ManejadorExcepcionesMiddleware
{
    private static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    private readonly RequestDelegate _siguiente;
    private readonly ILogger<ManejadorExcepcionesMiddleware> _logger;

    public ManejadorExcepcionesMiddleware(
        RequestDelegate siguiente,
        ILogger<ManejadorExcepcionesMiddleware> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (Exception excepcion)
        {
            if (contexto.Response.HasStarted)
            {
                throw;
            }

            await EscribirAsync(contexto, excepcion);
        }
    }

    private async Task EscribirAsync(HttpContext contexto, Exception excepcion)
    {
        var error = ErrorHttp.De(excepcion);

        if (error.Status >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(excepcion, "Error no controlado en {Metodo} {Ruta}",
                contexto.Request.Method,
                contexto.Request.Path);
        }
        else
        {
            _logger.LogWarning(excepcion, "{Titulo} en {Metodo} {Ruta}",
                error.Title,
                contexto.Request.Method,
                contexto.Request.Path);
        }

        var problema = error.AProblemDetails(excepcion.Message, contexto.Request.Path);
        problema.Extensions["traceId"] = contexto.TraceIdentifier;

        contexto.Response.Clear();
        contexto.Response.StatusCode = error.Status;
        contexto.Response.ContentType = "application/problem+json";

        await contexto.Response.WriteAsync(
            JsonSerializer.Serialize(problema, Json),
            contexto.RequestAborted);
    }
}
