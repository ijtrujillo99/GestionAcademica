using System.Data.Common;
using System.Text.Json;
using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Comun.Errores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace GestionAcademica.Api.Tests.Comun;

public sealed class MapeoExcepcionHttpTests
{
    [Fact]
    public void De_CuandoEsReglaDeNegocio_Devuelve400()
    {
        var error = ErrorHttp.De(new ReglaNegocioException(
            "La suma de puntajes (65.00) supera el máximo del parcial (50.00)."));

        Assert.Equal(StatusCodes.Status400BadRequest, error.Status);
        Assert.Equal("Regla de negocio", error.Title);
        Assert.Equal("https://httpstatuses.com/400", error.Type);
    }

    [Fact]
    public void De_CuandoNoExisteElRecurso_Devuelve404()
    {
        var error = ErrorHttp.De(new KeyNotFoundException("No existe la evaluación 99."));

        Assert.Equal(StatusCodes.Status404NotFound, error.Status);
        Assert.Equal("Recurso no encontrado", error.Title);
    }

    [Fact]
    public void De_CuandoFallaLaBase_Devuelve503()
    {
        var error = ErrorHttp.De(new BaseCaidaException("SQL Server no está disponible."));

        Assert.Equal(StatusCodes.Status503ServiceUnavailable, error.Status);
        Assert.Equal("Servicio no disponible", error.Title);
    }

    [Fact]
    public void De_CuandoEsInesperado_Devuelve500()
    {
        var error = ErrorHttp.De(new InvalidOperationException("fallo interno"));

        Assert.Equal(StatusCodes.Status500InternalServerError, error.Status);
        Assert.Equal("Error interno", error.Title);
    }

    [Theory]
    [InlineData(typeof(ReglaNegocioException), 400, "Regla de negocio")]
    [InlineData(typeof(KeyNotFoundException), 404, "Recurso no encontrado")]
    [InlineData(typeof(BaseCaidaException), 503, "Servicio no disponible")]
    [InlineData(typeof(Exception), 500, "Error interno")]
    public async Task Middleware_EscribeProblemDetailsConElMismoContrato(
        Type tipoExcepcion,
        int statusEsperado,
        string tituloEsperado)
    {
        var excepcion = (Exception)Activator.CreateInstance(tipoExcepcion, "detalle de prueba")!;
        var contexto = new DefaultHttpContext();
        contexto.Response.Body = new MemoryStream();
        contexto.Request.Path = "/api/evaluaciones/99";

        var middleware = new ManejadorExcepcionesMiddleware(
            _ => throw excepcion,
            NullLogger<ManejadorExcepcionesMiddleware>.Instance);

        await middleware.InvokeAsync(contexto);

        contexto.Response.Body.Position = 0;
        var json = await new StreamReader(contexto.Response.Body).ReadToEndAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal(statusEsperado, contexto.Response.StatusCode);
        Assert.Equal("application/problem+json", contexto.Response.ContentType);
        Assert.Equal(tituloEsperado, root.GetProperty("title").GetString());
        Assert.Equal(statusEsperado, root.GetProperty("status").GetInt32());
        Assert.Equal("detalle de prueba", root.GetProperty("detail").GetString());
        Assert.Equal("/api/evaluaciones/99", root.GetProperty("instance").GetString());
        Assert.True(root.TryGetProperty("traceId", out _));
    }

    private sealed class BaseCaidaException : DbException
    {
        public BaseCaidaException(string message)
            : base(message)
        {
        }
    }
}
