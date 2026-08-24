using GestionAcademica.Api.Cursos.Aplicacion;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Cursos.Datos;

public class CursosServicio : ICursosServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public CursosServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<CursoOfertadoResumenDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.CursosOfertados
            .AsNoTracking()
            .OrderBy(c => c.IdCursoOfertado)
            .Select(c => new CursoOfertadoResumenDto(
                c.IdCursoOfertado,
                c.Asignatura.Nombre,
                c.Grupo.CodigoGrupo,
                c.Docente.Nombre,
                c.Aula.CodigoAula,
                c.Inscripciones.Count,
                c.Horarios
                    .OrderBy(h => h.DiaSemana)
                    .ThenBy(h => h.HoraInicio)
                    .Select(h => new HorarioCursoDto(
                        h.IdHorario,
                        h.DiaSemana,
                        h.HoraInicio,
                        h.HoraFin))
                    .ToList()))
            .ToListAsync(cancellationToken);
    }
}
