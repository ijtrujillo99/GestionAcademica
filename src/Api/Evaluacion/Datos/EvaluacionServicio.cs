using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Infraestructura.Entidades.Eval;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.ModuloEvaluacion.Aplicacion;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.ModuloEvaluacion.Datos;

public class EvaluacionServicio : IEvaluacionServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public EvaluacionServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<ParcialResumenDto>> ListarParcialesAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        return await _contexto.Parciales
            .AsNoTracking()
            .Where(p => p.IdCursoOfertado == idCursoOfertado)
            .OrderBy(p => p.Orden)
            .Select(p => new ParcialResumenDto(
                p.IdParcial,
                p.Nombre,
                p.Orden,
                p.PuntajeMaximo,
                p.Evaluaciones.Count))
            .ToListAsync(cancellationToken);
    }

    public async Task<ParcialEvaluacionesDto> ListarEvaluacionesAsync(
        int idParcial,
        CancellationToken cancellationToken)
    {
        var parcial = await _contexto.Parciales
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdParcial == idParcial, cancellationToken);

        if (parcial is null)
        {
            throw new KeyNotFoundException($"No existe el parcial {idParcial}.");
        }

        var evaluaciones = await _contexto.Evaluaciones
            .AsNoTracking()
            .Where(e => e.IdParcial == idParcial)
            .OrderBy(e => e.Orden)
            .Select(e => new EvaluacionDetalleDto(
                e.IdEvaluacion,
                e.IdTipoEvaluacion,
                e.Nombre,
                e.TipoEvaluacion.Nombre,
                e.Descripcion,
                e.Semana,
                e.PuntajeMaximo,
                e.EsObligatoria,
                e.Orden))
            .ToListAsync(cancellationToken);

        return new ParcialEvaluacionesDto(
            parcial.IdParcial,
            parcial.Nombre,
            parcial.PuntajeMaximo,
            evaluaciones.Sum(e => e.PuntajeMaximo),
            evaluaciones);
    }

    public async Task<EvaluacionIndividualDto> ObtenerEvaluacionAsync(
        int idEvaluacion,
        CancellationToken cancellationToken)
    {
        var evaluacion = await _contexto.Evaluaciones
            .AsNoTracking()
            .Where(e => e.IdEvaluacion == idEvaluacion)
            .Select(e => new EvaluacionIndividualDto(
                e.IdEvaluacion,
                e.IdParcial,
                e.IdTipoEvaluacion,
                e.Nombre,
                e.TipoEvaluacion.Nombre,
                e.Descripcion,
                e.Semana,
                e.PuntajeMaximo,
                e.EsObligatoria,
                e.Orden))
            .FirstOrDefaultAsync(cancellationToken);

        if (evaluacion is null)
        {
            throw new KeyNotFoundException($"No existe la evaluación {idEvaluacion}.");
        }

        return evaluacion;
    }

    public async Task<IReadOnlyList<EvaluacionIndividualDto>> ListarTodasAsync(
        CancellationToken cancellationToken)
    {
        return await _contexto.Evaluaciones
            .AsNoTracking()
            .OrderBy(e => e.IdParcial)
            .ThenBy(e => e.Orden)
            .Select(e => new EvaluacionIndividualDto(
                e.IdEvaluacion,
                e.IdParcial,
                e.IdTipoEvaluacion,
                e.Nombre,
                e.TipoEvaluacion.Nombre,
                e.Descripcion,
                e.Semana,
                e.PuntajeMaximo,
                e.EsObligatoria,
                e.Orden))
            .ToListAsync(cancellationToken);
    }

    public async Task<ParcialEvaluacionesDto> CrearEvaluacionAsync(
        int idParcial,
        EvaluacionEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var parcial = await _contexto.Parciales
            .FirstOrDefaultAsync(p => p.IdParcial == idParcial, cancellationToken);

        if (parcial is null)
        {
            throw new KeyNotFoundException($"No existe el parcial {idParcial}.");
        }

        await ValidarEscrituraAsync(idParcial, excluirIdEvaluacion: null, datos, cancellationToken);

        var sumaActual = await _contexto.Evaluaciones
            .Where(e => e.IdParcial == idParcial)
            .SumAsync(e => (decimal?)e.PuntajeMaximo, cancellationToken) ?? 0;

        ValidarTopeParcial(sumaActual + datos.PuntajeMaximo, parcial.PuntajeMaximo);

        _contexto.Evaluaciones.Add(new Evaluacion
        {
            IdParcial = idParcial,
            IdTipoEvaluacion = datos.IdTipoEvaluacion,
            Nombre = datos.Nombre.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(datos.Descripcion) ? null : datos.Descripcion.Trim(),
            PuntajeMaximo = datos.PuntajeMaximo,
            EsObligatoria = datos.EsObligatoria,
            Orden = datos.Orden,
            Semana = datos.Semana
        });

        await _contexto.SaveChangesAsync(cancellationToken);

        return await ListarEvaluacionesAsync(idParcial, cancellationToken);
    }

    public async Task<ParcialEvaluacionesDto> EditarEvaluacionAsync(
        int idEvaluacion,
        EvaluacionEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var evaluacion = await _contexto.Evaluaciones
            .Include(e => e.Parcial)
            .FirstOrDefaultAsync(e => e.IdEvaluacion == idEvaluacion, cancellationToken);

        if (evaluacion is null)
        {
            throw new KeyNotFoundException($"No existe la evaluación {idEvaluacion}.");
        }

        await ValidarEscrituraAsync(evaluacion.IdParcial, idEvaluacion, datos, cancellationToken);

        var notaMayor = await _contexto.Calificaciones
            .Where(c => c.IdEvaluacion == idEvaluacion)
            .Select(c => (decimal?)c.PuntajeObtenido)
            .MaxAsync(cancellationToken);

        if (notaMayor.HasValue && notaMayor.Value > datos.PuntajeMaximo)
        {
            throw new ReglaNegocioException(
                $"No es posible reducir el puntaje máximo a {datos.PuntajeMaximo} porque existen calificaciones superiores a ese valor.");
        }

        var sumaSinEsta = await _contexto.Evaluaciones
            .Where(e => e.IdParcial == evaluacion.IdParcial && e.IdEvaluacion != idEvaluacion)
            .SumAsync(e => (decimal?)e.PuntajeMaximo, cancellationToken) ?? 0;

        ValidarTopeParcial(sumaSinEsta + datos.PuntajeMaximo, evaluacion.Parcial.PuntajeMaximo);

        evaluacion.IdTipoEvaluacion = datos.IdTipoEvaluacion;
        evaluacion.Nombre = datos.Nombre.Trim();
        evaluacion.Descripcion = string.IsNullOrWhiteSpace(datos.Descripcion) ? null : datos.Descripcion.Trim();
        evaluacion.PuntajeMaximo = datos.PuntajeMaximo;
        evaluacion.EsObligatoria = datos.EsObligatoria;
        evaluacion.Orden = datos.Orden;
        evaluacion.Semana = datos.Semana;

        await _contexto.SaveChangesAsync(cancellationToken);

        return await ListarEvaluacionesAsync(evaluacion.IdParcial, cancellationToken);
    }

    public async Task<EvaluacionCalificacionesDto> ObtenerCalificacionesAsync(
        int idEvaluacion,
        CancellationToken cancellationToken)
    {
        var evaluacion = await _contexto.Evaluaciones
            .AsNoTracking()
            .Include(e => e.Parcial)
            .FirstOrDefaultAsync(e => e.IdEvaluacion == idEvaluacion, cancellationToken);

        if (evaluacion is null)
        {
            throw new KeyNotFoundException($"No existe la evaluación {idEvaluacion}.");
        }

        var filas = await _contexto.Inscripciones
            .AsNoTracking()
            .Where(i => i.IdCursoOfertado == evaluacion.Parcial.IdCursoOfertado)
            .OrderBy(i => i.Estudiante.Nombre)
            .Select(i => new
            {
                i.IdInscripcion,
                i.IdEstudiante,
                i.Estudiante.Carnet,
                i.Estudiante.Nombre,
                i.EsOficial,
                EstadoInscripcion = i.EstadoInscripcion.Nombre,
                Nota = i.Calificaciones
                    .Where(c => c.IdEvaluacion == idEvaluacion)
                    .Select(c => new { c.PuntajeObtenido, c.Observacion })
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return new EvaluacionCalificacionesDto(
            evaluacion.IdEvaluacion,
            evaluacion.IdParcial,
            evaluacion.Parcial.IdCursoOfertado,
            evaluacion.Nombre,
            evaluacion.PuntajeMaximo,
            evaluacion.EsObligatoria,
            filas.Select(f => new CalificacionFilaDto(
                f.IdInscripcion,
                f.IdEstudiante,
                f.Carnet,
                f.Nombre,
                f.EsOficial,
                f.EstadoInscripcion,
                f.Nota?.PuntajeObtenido,
                f.Nota?.Observacion)).ToList());
    }

    public async Task<EvaluacionCalificacionesDto> GuardarCalificacionesAsync(
        int idEvaluacion,
        IReadOnlyList<CalificacionCambioDto> cambios,
        CancellationToken cancellationToken)
    {
        var evaluacion = await _contexto.Evaluaciones
            .Include(e => e.Parcial)
            .FirstOrDefaultAsync(e => e.IdEvaluacion == idEvaluacion, cancellationToken);

        if (evaluacion is null)
        {
            throw new KeyNotFoundException($"No existe la evaluación {idEvaluacion}.");
        }

        if (cambios.Select(c => c.IdInscripcion).Distinct().Count() != cambios.Count)
        {
            throw new ReglaNegocioException("Hay inscripciones duplicadas en la lista.");
        }

        var inscripcionesCurso = await _contexto.Inscripciones
            .Where(i => i.IdCursoOfertado == evaluacion.Parcial.IdCursoOfertado)
            .Select(i => i.IdInscripcion)
            .ToListAsync(cancellationToken);

        var delCurso = inscripcionesCurso.ToHashSet();

        foreach (var cambio in cambios)
        {
            if (!delCurso.Contains(cambio.IdInscripcion))
            {
                throw new ReglaNegocioException(
                    $"La inscripción {cambio.IdInscripcion} no pertenece a este curso.");
            }

            if (cambio.PuntajeObtenido < 0 || cambio.PuntajeObtenido > evaluacion.PuntajeMaximo)
            {
                throw new ReglaNegocioException(
                    $"La nota de la inscripción {cambio.IdInscripcion} debe estar entre 0 y {evaluacion.PuntajeMaximo}.");
            }
        }

        var existentes = await _contexto.Calificaciones
            .Where(c => c.IdEvaluacion == idEvaluacion)
            .ToListAsync(cancellationToken);

        var porInscripcion = existentes.ToDictionary(c => c.IdInscripcion);
        var ahora = DateTime.Now;

        foreach (var cambio in cambios)
        {
            if (porInscripcion.TryGetValue(cambio.IdInscripcion, out var fila))
            {
                fila.PuntajeObtenido = cambio.PuntajeObtenido;
                fila.Observacion = cambio.Observacion;
                fila.FechaRegistro = ahora;
            }
            else
            {
                _contexto.Calificaciones.Add(new Calificacion
                {
                    IdEvaluacion = idEvaluacion,
                    IdInscripcion = cambio.IdInscripcion,
                    PuntajeObtenido = cambio.PuntajeObtenido,
                    Observacion = cambio.Observacion,
                    FechaRegistro = ahora
                });
            }
        }

        await _contexto.SaveChangesAsync(cancellationToken);

        return await ObtenerCalificacionesAsync(idEvaluacion, cancellationToken);
    }

    private async Task ValidarEscrituraAsync(
        int idParcial,
        int? excluirIdEvaluacion,
        EvaluacionEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(datos.Nombre))
        {
            throw new ReglaNegocioException("El nombre de la evaluación es obligatorio.");
        }

        if (datos.PuntajeMaximo <= 0)
        {
            throw new ReglaNegocioException("El puntaje máximo debe ser mayor que 0.");
        }

        if (datos.Orden == 0)
        {
            throw new ReglaNegocioException("El orden debe ser mayor que 0.");
        }

        var tipoExiste = await _contexto.TiposEvaluacion
            .AnyAsync(t => t.IdTipoEvaluacion == datos.IdTipoEvaluacion && t.Activo, cancellationToken);

        if (!tipoExiste)
        {
            throw new ReglaNegocioException($"El tipo de evaluación {datos.IdTipoEvaluacion} no es válido.");
        }

        var ordenOcupado = await _contexto.Evaluaciones
            .AnyAsync(
                e => e.IdParcial == idParcial
                     && e.Orden == datos.Orden
                     && e.IdEvaluacion != (excluirIdEvaluacion ?? 0),
                cancellationToken);

        if (ordenOcupado)
        {
            throw new ReglaNegocioException($"Ya existe una evaluación con orden {datos.Orden} en este parcial.");
        }
    }

    private static void ValidarTopeParcial(decimal sumaPropuesta, decimal tope)
    {
        if (sumaPropuesta > tope)
        {
            throw new ReglaNegocioException(
                $"La suma de puntajes ({sumaPropuesta}) supera el máximo del parcial ({tope}).");
        }
    }
}
