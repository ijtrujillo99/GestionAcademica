using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.ModuloAsistencia.Aplicacion;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.ModuloAsistencia.Datos;

public class AsistenciaServicio : IAsistenciaServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public AsistenciaServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<ClaseResumenDto>> ListarClasesAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return await _contexto.Clases
            .AsNoTracking()
            .Where(c => c.IdCursoOfertado == idCursoOfertado)
            .OrderBy(c => c.Fecha)
            .Select(c => new ClaseResumenDto(
                c.IdClase,
                c.Fecha,
                c.Tema,
                c.Asistencias.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<ClaseAsistenciaDto> ObtenerListaAsync(int idClase, CancellationToken cancellationToken)
    {
        var clase = await _contexto.Clases
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdClase == idClase, cancellationToken);

        if (clase is null)
        {
            throw new KeyNotFoundException($"No existe la clase {idClase}.");
        }

        var fechaClase = clase.Fecha.Date;

        var estados = await _contexto.EstadosAsistencia
            .AsNoTracking()
            .Where(e => e.Activo)
            .OrderBy(e => e.IdEstadoAsistencia)
            .Select(e => new EstadoAsistenciaDto(e.IdEstadoAsistencia, e.Nombre))
            .ToListAsync(cancellationToken);

        var filas = await _contexto.Inscripciones
            .AsNoTracking()
            .Where(i => i.IdCursoOfertado == clase.IdCursoOfertado)
            .Where(i => i.FechaInicio.Date <= fechaClase)
            .Where(i => i.FechaRetiro == null || fechaClase < i.FechaRetiro.Value.Date)
            .OrderBy(i => i.Estudiante.Nombre)
            .Select(i => new
            {
                i.IdInscripcion,
                i.IdEstudiante,
                i.Estudiante.Carnet,
                i.Estudiante.Nombre,
                i.Estudiante.Sexo,
                i.EsOficial,
                EstadoInscripcion = i.EstadoInscripcion.Nombre,
                Registro = i.Asistencias
                    .Where(a => a.IdClase == idClase)
                    .Select(a => new
                    {
                        a.IdEstadoAsistencia,
                        EstadoAsistencia = a.EstadoAsistencia.Nombre,
                        a.Observacion
                    })
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return new ClaseAsistenciaDto(
            clase.IdClase,
            clase.IdCursoOfertado,
            clase.Fecha,
            clase.Tema,
            estados,
            filas.Select(f => new AsistenciaFilaDto(
                f.IdInscripcion,
                f.IdEstudiante,
                f.Carnet,
                f.Nombre,
                f.Sexo,
                f.EsOficial,
                f.EstadoInscripcion,
                f.Registro?.IdEstadoAsistencia,
                f.Registro?.EstadoAsistencia,
                f.Registro?.Observacion)).ToList());
    }

    public async Task<ClaseAsistenciaDto> GuardarAsync(
        int idClase,
        IReadOnlyList<AsistenciaCambioDto> cambios,
        CancellationToken cancellationToken)
    {
        var clase = await _contexto.Clases
            .FirstOrDefaultAsync(c => c.IdClase == idClase, cancellationToken);

        if (clase is null)
        {
            throw new KeyNotFoundException($"No existe la clase {idClase}.");
        }

        if (cambios.Select(c => c.IdInscripcion).Distinct().Count() != cambios.Count)
        {
            throw new ReglaNegocioException("Hay inscripciones duplicadas en la lista.");
        }

        var fechaClase = clase.Fecha.Date;

        var estadosValidos = await _contexto.EstadosAsistencia
            .AsNoTracking()
            .Where(e => e.Activo)
            .Select(e => e.IdEstadoAsistencia)
            .ToListAsync(cancellationToken);

        var inscripciones = await _contexto.Inscripciones
            .Where(i => i.IdCursoOfertado == clase.IdCursoOfertado)
            .ToListAsync(cancellationToken);

        var porId = inscripciones.ToDictionary(i => i.IdInscripcion);

        foreach (var cambio in cambios)
        {
            if (!estadosValidos.Contains(cambio.IdEstadoAsistencia))
            {
                throw new ReglaNegocioException(
                    $"El estado de asistencia {cambio.IdEstadoAsistencia} no es válido.");
            }

            if (!porId.TryGetValue(cambio.IdInscripcion, out var inscripcion))
            {
                throw new ReglaNegocioException(
                    $"La inscripción {cambio.IdInscripcion} no pertenece a este curso.");
            }

            if (!EsEsperada(inscripcion, fechaClase))
            {
                throw new ReglaNegocioException(
                    $"La inscripción {cambio.IdInscripcion} no corresponde asistencia en esta sesión (retiro o fecha de inicio).");
            }
        }

        var existentes = await _contexto.Asistencias
            .Where(a => a.IdClase == idClase)
            .ToListAsync(cancellationToken);

        var existentesPorInscripcion = existentes.ToDictionary(a => a.IdInscripcion);

        foreach (var cambio in cambios)
        {
            if (existentesPorInscripcion.TryGetValue(cambio.IdInscripcion, out var fila))
            {
                fila.IdEstadoAsistencia = cambio.IdEstadoAsistencia;
                fila.Observacion = cambio.Observacion;
            }
            else
            {
                _contexto.Asistencias.Add(new Asistencia
                {
                    IdClase = idClase,
                    IdInscripcion = cambio.IdInscripcion,
                    IdEstadoAsistencia = cambio.IdEstadoAsistencia,
                    Observacion = cambio.Observacion
                });
            }
        }

        await _contexto.SaveChangesAsync(cancellationToken);

        return await ObtenerListaAsync(idClase, cancellationToken);
    }

    private static bool EsEsperada(InscripcionCurso inscripcion, DateTime fechaClase)
    {
        if (inscripcion.FechaInicio.Date > fechaClase)
        {
            return false;
        }

        return inscripcion.FechaRetiro is null
            || fechaClase < inscripcion.FechaRetiro.Value.Date;
    }
}
