namespace GestionAcademica.Api.Reportes.Aplicacion;

public sealed class SituacionAcademicaServicio : ISituacionAcademicaServicio
{
    private readonly IResumenSituacionConsulta _consulta;
    private readonly IClasificadorResultadoAcademico _clasificador;

    public SituacionAcademicaServicio(
        IResumenSituacionConsulta consulta,
        IClasificadorResultadoAcademico clasificador)
    {
        _consulta = consulta;
        _clasificador = clasificador;
    }

    public async Task<IReadOnlyList<SituacionAcademicaEstudianteDto>> ObtenerAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        var resumenes = await _consulta.ObtenerPorCursoAsync(idCursoOfertado, cancellationToken);

        return resumenes
            .Select(resumen => new SituacionAcademicaEstudianteDto(
                resumen.IdInscripcion,
                resumen.IdEstudiante,
                resumen.Carnet,
                resumen.Nombre,
                resumen.EstadoInscripcion,
                resumen.ParcialI,
                resumen.ParcialII,
                resumen.PuntajeTareaCurso,
                resumen.EvaluacionesPendientes,
                resumen.NotaFinal,
                ResultadoAcademico.NotaMinimaAprobacion,
                _clasificador.Clasificar(resumen)))
            .ToList();
    }
}
