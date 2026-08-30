using GestionAcademica.Api.Inscripciones.Aplicacion;

namespace GestionAcademica.Api.Reportes.Aplicacion;

/// <summary>
/// Replica la regla de ceros de <c>eval.sp_ReporteCalificaciones</c>
/// para retirados sin notas reales. El GET sigue resolviendo el
/// reporte en SQL; esta clase documenta y permite probar la regla.
/// </summary>
public static class ReglasReporteCalificaciones
{
    public const string ColumnaEstadoInscripcion = "EstadoInscripcion";
    public const string ColumnaIdEstudiante = "IdEstudiante";
    public const string ColumnaIdInscripcion = "IdInscripcion";

    /// <summary>
    /// Texto de catálogo. Coincide con inscripciones y situación
    /// académica. No es <c>Retirado</c> (eso es el resultado académico).
    /// </summary>
    public const string EstadoRetirada = ReglasInscripcion.Retirada;

    public const string EstadoActiva = ReglasInscripcion.Activa;

    /// <summary>
    /// Si es <c>Retirada</c> y no hay ninguna nota distinta de 0
    /// (sin fila o todas en 0 / sin evaluar), el SP pone ceros en
    /// las evaluaciones y <c>AcumuladoTotal = 0</c>.
    /// </summary>
    public static bool DebePonerCeros(string estadoInscripcion, IEnumerable<decimal?> notas)
    {
        if (!ReglasInscripcion.EsRetirada(estadoInscripcion))
        {
            return false;
        }

        return notas.All(nota => nota is null || nota == 0);
    }
}
