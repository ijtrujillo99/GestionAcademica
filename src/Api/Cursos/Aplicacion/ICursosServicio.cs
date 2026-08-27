namespace GestionAcademica.Api.Cursos.Aplicacion;

public interface ICursosServicio
{
    Task<IReadOnlyList<CursoOfertadoResumenDto>> ListarAsync(CancellationToken cancellationToken);

    Task<CursoOfertadoDetalleDto> ObtenerAsync(int idCursoOfertado, CancellationToken cancellationToken);

    Task<CursoOfertadoDetalleDto> CrearAsync(
        CursoOfertadoEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<CursoOfertadoDetalleDto> EditarAsync(
        int idCursoOfertado,
        CursoOfertadoEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<HorarioCursoDto>> ListarHorariosAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<HorarioCursoDto>> CrearHorarioAsync(
        int idCursoOfertado,
        HorarioEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<HorarioCursoDto> EditarHorarioAsync(
        int idHorario,
        HorarioEscrituraDto datos,
        CancellationToken cancellationToken);
}

public record HorarioCursoDto(
    int IdHorario,
    byte DiaSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraFin);

public record HorarioEscrituraDto(
    byte DiaSemana,
    TimeSpan HoraInicio,
    TimeSpan HoraFin);

public record CursoOfertadoResumenDto(
    int IdCursoOfertado,
    string Asignatura,
    string Grupo,
    int IdCarrera,
    string Carrera,
    string Docente,
    string Aula,
    int Inscripciones,
    IReadOnlyList<HorarioCursoDto> Horarios);

public record CursoOfertadoDetalleDto(
    int IdCursoOfertado,
    int IdAsignatura,
    string Asignatura,
    int IdGrupo,
    string Grupo,
    int IdCarrera,
    string Carrera,
    int IdDocente,
    string Docente,
    int IdAula,
    string Aula,
    short? CupoMaximo,
    bool Activo,
    IReadOnlyList<HorarioCursoDto> Horarios);

public record CursoOfertadoEscrituraDto(
    int IdAsignatura,
    int IdGrupo,
    int IdDocente,
    int IdAula,
    short? CupoMaximo,
    bool Activo = true);
