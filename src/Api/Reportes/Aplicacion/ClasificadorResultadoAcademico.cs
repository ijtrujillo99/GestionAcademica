namespace GestionAcademica.Api.Reportes.Aplicacion;

/// <summary>
/// Replica el CASE de <c>eval.sp_ReporteSituacionAcademica</c>.
/// El endpoint GET sigue resolviendo el reporte en SQL; esta clase
/// documenta y permite probar las mismas reglas en C#.
/// </summary>
public sealed class ClasificadorResultadoAcademico : IClasificadorResultadoAcademico
{
    public string Clasificar(ResumenSituacionEstudiante resumen)
    {
        if (EsRetirado(resumen.EstadoInscripcion))
        {
            return ResultadoAcademico.Retirado;
        }

        if (resumen.EvaluacionesPendientes > 0)
        {
            return ResultadoAcademico.EnCurso;
        }

        if (resumen.NotaFinal < ResultadoAcademico.NotaMinimaAprobacion)
        {
            return ResultadoAcademico.Reprobado;
        }

        if (resumen.PuntajeTareaCurso is null)
        {
            return ResultadoAcademico.EnCurso;
        }

        if (resumen.PuntajeTareaCurso == 0)
        {
            return ResultadoAcademico.NoAprobadoTareaCurso;
        }

        return ResultadoAcademico.Aprobado;
    }

    private static bool EsRetirado(string estadoInscripcion)
    {
        return string.Equals(
            estadoInscripcion,
            ResultadoAcademico.EstadoInscripcionRetirada,
            StringComparison.OrdinalIgnoreCase);
    }
}
