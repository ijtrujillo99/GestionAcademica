namespace GestionAcademica.Api.Reportes.Aplicacion;

/// <summary>
/// Textos y umbrales del resultado académico.
/// Deben coincidir con <c>eval.sp_ReporteSituacionAcademica</c>.
/// </summary>
public static class ResultadoAcademico
{
    public const string Retirado = "Retirado";
    public const string EnCurso = "En curso";
    public const string Aprobado = "Aprobado";
    public const string Reprobado = "Reprobado";
    public const string NoAprobadoTareaCurso = "No aprobado - Tarea de Curso";

    public const decimal NotaMinimaAprobacion = 60m;
    public const string EstadoInscripcionRetirada = "Retirada";
    public const string EstadoInscripcionActiva = "Activa";
}

/// <summary>
/// Entrada alineada con el SP: <see cref="NotaFinal"/> es Parcial I + Parcial II
/// (tope 50 + 50). <see cref="PuntajeTareaCurso"/> no se suma otra vez;
/// ya va dentro del Parcial II y solo decide la condición obligatoria.
/// </summary>
public sealed record ResumenSituacionEstudiante(
    int IdInscripcion,
    int IdEstudiante,
    string Carnet,
    string Nombre,
    string EstadoInscripcion,
    int EvaluacionesPendientes,
    decimal ParcialI,
    decimal ParcialII,
    decimal? PuntajeTareaCurso)
{
    public decimal NotaFinal => ParcialI + ParcialII;
}

public sealed record SituacionAcademicaEstudianteDto(
    int IdInscripcion,
    int IdEstudiante,
    string Carnet,
    string Nombre,
    string EstadoInscripcion,
    decimal ParcialI,
    decimal ParcialII,
    decimal? PuntajeTareaCurso,
    int EvaluacionesPendientes,
    decimal NotaFinal,
    decimal NotaMinimaAprobacion,
    string ResultadoAcademico);

public interface IClasificadorResultadoAcademico
{
    string Clasificar(ResumenSituacionEstudiante resumen);
}

public interface IResumenSituacionConsulta
{
    Task<IReadOnlyList<ResumenSituacionEstudiante>> ObtenerPorCursoAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken);
}

public interface ISituacionAcademicaServicio
{
    Task<IReadOnlyList<SituacionAcademicaEstudianteDto>> ObtenerAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken);
}
