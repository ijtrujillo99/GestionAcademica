using GestionAcademica.Api.Reportes.Aplicacion;
using Microsoft.AspNetCore.Mvc;

namespace GestionAcademica.Api.Reportes.Api;

[ApiController]
[Route("api/cursos-ofertados/{idCursoOfertado:int}/reportes")]
public class ReportesController : ControllerBase
{
    private readonly IReportesServicio _servicio;

    public ReportesController(IReportesServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet("asistencia-estudiante")]
    public async Task<IActionResult> AsistenciaEstudiante(
        int idCursoOfertado,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.AsistenciaEstudianteAsync(
            idCursoOfertado, fechaDesde, fechaHasta, cancellationToken));
    }

    [HttpGet("asistencia-sesion")]
    public async Task<IActionResult> AsistenciaSesion(
        int idCursoOfertado,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.AsistenciaSesionAsync(
            idCursoOfertado, fechaDesde, fechaHasta, cancellationToken));
    }

    [HttpGet("asistencia-semanal")]
    public async Task<IActionResult> AsistenciaSemanal(
        int idCursoOfertado,
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.AsistenciaSemanalAsync(
            idCursoOfertado, fechaDesde, fechaHasta, cancellationToken));
    }

    [HttpGet("calificaciones")]
    public async Task<IActionResult> Calificaciones(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.CalificacionesAsync(idCursoOfertado, cancellationToken));
    }

    [HttpGet("situacion-academica")]
    public async Task<IActionResult> SituacionAcademica(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return Ok(await _servicio.SituacionAcademicaAsync(idCursoOfertado, cancellationToken));
    }

    [HttpGet("acumulado-notas")]
    public async Task<IActionResult> AcumuladoNotas(
        int idCursoOfertado,
        [FromQuery] bool incluirAcumuladoParcialI = true,
        [FromQuery] bool incluirExamenParcialI = false,
        [FromQuery] bool incluirAcumuladoParcialII = false,
        [FromQuery] bool incluirExamenParcialII = false,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _servicio.AcumuladoNotasAsync(
            idCursoOfertado,
            incluirAcumuladoParcialI,
            incluirExamenParcialI,
            incluirAcumuladoParcialII,
            incluirExamenParcialII,
            cancellationToken));
    }
}
