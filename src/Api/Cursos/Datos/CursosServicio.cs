using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Cursos.Aplicacion;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
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
                c.Grupo.IdCarrera,
                c.Grupo.Carrera.Nombre,
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

    public async Task<CursoOfertadoDetalleDto> ObtenerAsync(int idCursoOfertado, CancellationToken cancellationToken)
    {
        var curso = await _contexto.CursosOfertados
            .AsNoTracking()
            .Where(c => c.IdCursoOfertado == idCursoOfertado)
            .Select(c => new CursoOfertadoDetalleDto(
                c.IdCursoOfertado,
                c.IdAsignatura,
                c.Asignatura.Nombre,
                c.IdGrupo,
                c.Grupo.CodigoGrupo,
                c.Grupo.IdCarrera,
                c.Grupo.Carrera.Nombre,
                c.IdDocente,
                c.Docente.Nombre,
                c.IdAula,
                c.Aula.CodigoAula,
                c.CupoMaximo,
                c.Activo,
                c.Horarios
                    .OrderBy(h => h.DiaSemana)
                    .ThenBy(h => h.HoraInicio)
                    .Select(h => new HorarioCursoDto(
                        h.IdHorario,
                        h.DiaSemana,
                        h.HoraInicio,
                        h.HoraFin))
                    .ToList()))
            .FirstOrDefaultAsync(cancellationToken);

        if (curso is null)
        {
            throw new KeyNotFoundException($"No existe el curso ofertado {idCursoOfertado}.");
        }

        return curso;
    }

    public async Task<CursoOfertadoDetalleDto> CrearAsync(
        CursoOfertadoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        await ValidarReferenciasAsync(datos, cancellationToken);
        ValidarCupo(datos.CupoMaximo);
        await AsegurarOfertaLibreAsync(datos.IdAsignatura, datos.IdGrupo, excluirId: null, cancellationToken);

        var curso = new CursoOfertado
        {
            IdAsignatura = datos.IdAsignatura,
            IdGrupo = datos.IdGrupo,
            IdDocente = datos.IdDocente,
            IdAula = datos.IdAula,
            CupoMaximo = datos.CupoMaximo,
            Activo = datos.Activo
        };

        _contexto.CursosOfertados.Add(curso);
        await _contexto.SaveChangesAsync(cancellationToken);
        return await ObtenerAsync(curso.IdCursoOfertado, cancellationToken);
    }

    public async Task<CursoOfertadoDetalleDto> EditarAsync(
        int idCursoOfertado,
        CursoOfertadoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var curso = await _contexto.CursosOfertados
            .FirstOrDefaultAsync(c => c.IdCursoOfertado == idCursoOfertado, cancellationToken);

        if (curso is null)
        {
            throw new KeyNotFoundException($"No existe el curso ofertado {idCursoOfertado}.");
        }

        await ValidarReferenciasAsync(datos, cancellationToken);
        ValidarCupo(datos.CupoMaximo);
        await AsegurarOfertaLibreAsync(datos.IdAsignatura, datos.IdGrupo, idCursoOfertado, cancellationToken);

        curso.IdAsignatura = datos.IdAsignatura;
        curso.IdGrupo = datos.IdGrupo;
        curso.IdDocente = datos.IdDocente;
        curso.IdAula = datos.IdAula;
        curso.CupoMaximo = datos.CupoMaximo;
        curso.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return await ObtenerAsync(idCursoOfertado, cancellationToken);
    }

    public async Task<IReadOnlyList<HorarioCursoDto>> ListarHorariosAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        await AsegurarCursoAsync(idCursoOfertado, cancellationToken);
        return await ListarHorariosInternoAsync(idCursoOfertado, cancellationToken);
    }

    public async Task<IReadOnlyList<HorarioCursoDto>> CrearHorarioAsync(
        int idCursoOfertado,
        HorarioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        await AsegurarCursoAsync(idCursoOfertado, cancellationToken);
        ValidacionHorario.Validar(datos.DiaSemana, datos.HoraInicio, datos.HoraFin);
        await AsegurarBloqueLibreAsync(
            idCursoOfertado,
            datos,
            excluirId: null,
            cancellationToken);

        _contexto.Horarios.Add(new Horario
        {
            IdCursoOfertado = idCursoOfertado,
            DiaSemana = datos.DiaSemana,
            HoraInicio = datos.HoraInicio,
            HoraFin = datos.HoraFin
        });

        await _contexto.SaveChangesAsync(cancellationToken);
        return await ListarHorariosInternoAsync(idCursoOfertado, cancellationToken);
    }

    public async Task<HorarioCursoDto> EditarHorarioAsync(
        int idHorario,
        HorarioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var horario = await _contexto.Horarios
            .FirstOrDefaultAsync(h => h.IdHorario == idHorario, cancellationToken);

        if (horario is null)
        {
            throw new KeyNotFoundException($"No existe el horario {idHorario}.");
        }

        ValidacionHorario.Validar(datos.DiaSemana, datos.HoraInicio, datos.HoraFin);
        await AsegurarBloqueLibreAsync(horario.IdCursoOfertado, datos, idHorario, cancellationToken);

        horario.DiaSemana = datos.DiaSemana;
        horario.HoraInicio = datos.HoraInicio;
        horario.HoraFin = datos.HoraFin;

        await _contexto.SaveChangesAsync(cancellationToken);

        return new HorarioCursoDto(
            horario.IdHorario,
            horario.DiaSemana,
            horario.HoraInicio,
            horario.HoraFin);
    }

    private async Task AsegurarCursoAsync(int idCursoOfertado, CancellationToken cancellationToken)
    {
        var existe = await _contexto.CursosOfertados
            .AsNoTracking()
            .AnyAsync(c => c.IdCursoOfertado == idCursoOfertado, cancellationToken);

        if (!existe)
        {
            throw new KeyNotFoundException($"No existe el curso ofertado {idCursoOfertado}.");
        }
    }

    private async Task<IReadOnlyList<HorarioCursoDto>> ListarHorariosInternoAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return await _contexto.Horarios
            .AsNoTracking()
            .Where(h => h.IdCursoOfertado == idCursoOfertado)
            .OrderBy(h => h.DiaSemana)
            .ThenBy(h => h.HoraInicio)
            .Select(h => new HorarioCursoDto(h.IdHorario, h.DiaSemana, h.HoraInicio, h.HoraFin))
            .ToListAsync(cancellationToken);
    }

    private async Task ValidarReferenciasAsync(CursoOfertadoEscrituraDto datos, CancellationToken cancellationToken)
    {
        var asignatura = await _contexto.Asignaturas
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAsignatura == datos.IdAsignatura, cancellationToken);
        if (asignatura is null)
        {
            throw new KeyNotFoundException($"No existe la asignatura {datos.IdAsignatura}.");
        }

        if (!asignatura.Activo)
        {
            throw new ReglaNegocioException("La asignatura no está activa.");
        }

        var grupo = await _contexto.Grupos
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.IdGrupo == datos.IdGrupo, cancellationToken);
        if (grupo is null)
        {
            throw new KeyNotFoundException($"No existe el grupo {datos.IdGrupo}.");
        }

        if (!grupo.Activo)
        {
            throw new ReglaNegocioException("El grupo no está activo.");
        }

        var docente = await _contexto.Docentes
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.IdDocente == datos.IdDocente, cancellationToken);
        if (docente is null)
        {
            throw new KeyNotFoundException($"No existe el docente {datos.IdDocente}.");
        }

        if (!docente.Activo)
        {
            throw new ReglaNegocioException("El docente no está activo.");
        }

        var aula = await _contexto.Aulas
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAula == datos.IdAula, cancellationToken);
        if (aula is null)
        {
            throw new KeyNotFoundException($"No existe el aula {datos.IdAula}.");
        }

        if (!aula.Activo)
        {
            throw new ReglaNegocioException("El aula no está activa.");
        }
    }

    private async Task AsegurarOfertaLibreAsync(
        int idAsignatura,
        int idGrupo,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.CursosOfertados
            .AsNoTracking()
            .AnyAsync(
                c => c.IdAsignatura == idAsignatura
                     && c.IdGrupo == idGrupo
                     && c.IdCursoOfertado != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException(
                "Ya existe un curso ofertado para esa asignatura y grupo.");
        }
    }

    private async Task AsegurarBloqueLibreAsync(
        int idCursoOfertado,
        HorarioEscrituraDto datos,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.Horarios
            .AsNoTracking()
            .AnyAsync(
                h => h.IdCursoOfertado == idCursoOfertado
                     && h.DiaSemana == datos.DiaSemana
                     && h.HoraInicio == datos.HoraInicio
                     && h.HoraFin == datos.HoraFin
                     && h.IdHorario != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException(
                "Ya existe un bloque de horario igual para este curso.");
        }
    }

    private static void ValidarCupo(short? cupoMaximo)
    {
        if (cupoMaximo is <= 0)
        {
            throw new ReglaNegocioException("El cupo máximo debe ser mayor que 0.");
        }
    }
}
