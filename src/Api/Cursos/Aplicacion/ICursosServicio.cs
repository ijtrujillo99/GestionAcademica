namespace GestionAcademica.Api.Cursos.Aplicacion;

public interface ICursosServicio
{
    Task<IReadOnlyList<CursoOfertadoResumenDto>> ListarAsync(CancellationToken cancellationToken);
}

public record HorarioCursoDto(
    int IdHorario,
    byte DiaSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraFin);

public record CursoOfertadoResumenDto(
    int IdCursoOfertado,
    string Asignatura,
    string Grupo,
    string Docente,
    string Aula,
    int Inscripciones,
    IReadOnlyList<HorarioCursoDto> Horarios);
