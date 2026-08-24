using GestionAcademica.Api.Comun;

namespace GestionAcademica.Api.Inscripciones.Aplicacion;

public static class ReglasInscripcion
{
    public const string Activa = "Activa";
    public const string Pendiente = "Pendiente";
    public const string Retirada = "Retirada";

    public static bool EsRetirada(string nombre)
    {
        return string.Equals(nombre, Retirada, StringComparison.OrdinalIgnoreCase);
    }

    public static (DateTime? FechaRetiro, DateTime FechaEstado) AplicarEstado(
        string estadoNuevo,
        DateTime fechaInicio,
        DateTime? fechaRetiroActual,
        DateTime ahora)
    {
        DateTime? fechaRetiro = EsRetirada(estadoNuevo)
            ? (fechaRetiroActual ?? ahora.Date)
            : null;

        if (fechaRetiro.HasValue && fechaRetiro.Value.Date < fechaInicio.Date)
        {
            throw new ReglaNegocioException(
                "La fecha de retiro no puede ser anterior a la fecha de inicio.");
        }

        return (fechaRetiro, ahora);
    }
}
