using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GestionAcademica.Api.Comun.Errores;

public sealed record ErrorHttp(int Status, string Title, string Type)
{
    public static ErrorHttp De(Exception excepcion)
    {
        return excepcion switch
        {
            ReglaNegocioException => new ErrorHttp(
                StatusCodes.Status400BadRequest,
                "Regla de negocio",
                Tipo(StatusCodes.Status400BadRequest)),

            KeyNotFoundException => new ErrorHttp(
                StatusCodes.Status404NotFound,
                "Recurso no encontrado",
                Tipo(StatusCodes.Status404NotFound)),

            SqlException or DbException => new ErrorHttp(
                StatusCodes.Status503ServiceUnavailable,
                "Servicio no disponible",
                Tipo(StatusCodes.Status503ServiceUnavailable)),

            _ => new ErrorHttp(
                StatusCodes.Status500InternalServerError,
                "Error interno",
                Tipo(StatusCodes.Status500InternalServerError))
        };
    }

    public ProblemDetails AProblemDetails(string detail, string? instance = null)
    {
        return new ProblemDetails
        {
            Type = Type,
            Title = Title,
            Status = Status,
            Detail = detail,
            Instance = instance
        };
    }

    public static string Tipo(int status) => $"https://httpstatuses.com/{status}";
}
