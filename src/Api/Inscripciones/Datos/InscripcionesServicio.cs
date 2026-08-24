using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.Inscripciones.Aplicacion;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Inscripciones.Datos;

public class InscripcionesServicio : IInscripcionesServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public InscripcionesServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<CursoInscripcionesDto> ListarAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        await AsegurarCursoAsync(idCursoOfertado, cancellationToken);
        return await ConstruirListadoAsync(idCursoOfertado, cancellationToken);
    }

    public async Task<CursoInscripcionesDto> CrearAsync(
        int idCursoOfertado,
        InscripcionEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        await AsegurarCursoAsync(idCursoOfertado, cancellationToken);

        var estudiante = await _contexto.Estudiantes
            .FirstOrDefaultAsync(e => e.IdEstudiante == datos.IdEstudiante, cancellationToken);

        if (estudiante is null)
        {
            throw new KeyNotFoundException($"No existe el estudiante {datos.IdEstudiante}.");
        }

        if (!estudiante.Activo)
        {
            throw new ReglaNegocioException(
                "No es posible inscribir a un estudiante inactivo.");
        }

        var yaInscrito = await _contexto.Inscripciones
            .AsNoTracking()
            .AnyAsync(
                i => i.IdCursoOfertado == idCursoOfertado
                     && i.IdEstudiante == datos.IdEstudiante,
                cancellationToken);

        if (yaInscrito)
        {
            throw new ReglaNegocioException(
                "El estudiante ya está inscrito en este curso. Use la edición de la inscripción.");
        }

        var estado = await ResolverEstadoAltaAsync(datos.IdEstadoInscripcion, cancellationToken);
        var ahora = DateTime.Now;
        var (fechaRetiro, fechaEstado) = ReglasInscripcion.AplicarEstado(
            estado.Nombre,
            ahora.Date,
            fechaRetiroActual: null,
            ahora);

        _contexto.Inscripciones.Add(new InscripcionCurso
        {
            IdEstudiante = datos.IdEstudiante,
            IdCursoOfertado = idCursoOfertado,
            IdEstadoInscripcion = estado.IdEstadoInscripcion,
            FechaInicio = ahora.Date,
            FechaEstado = fechaEstado,
            FechaRetiro = fechaRetiro,
            EsOficial = datos.EsOficial ?? true,
            Observacion = NormalizarOpcional(datos.Observacion, 500, "observación")
        });

        await _contexto.SaveChangesAsync(cancellationToken);

        return await ConstruirListadoAsync(idCursoOfertado, cancellationToken);
    }

    public async Task<CursoInscripcionesDto> EditarAsync(
        int idInscripcion,
        InscripcionEdicionDto datos,
        CancellationToken cancellationToken)
    {
        var inscripcion = await _contexto.Inscripciones
            .FirstOrDefaultAsync(i => i.IdInscripcion == idInscripcion, cancellationToken);

        if (inscripcion is null)
        {
            throw new KeyNotFoundException($"No existe la inscripción {idInscripcion}.");
        }

        var estado = await ObtenerEstadoActivoAsync(datos.IdEstadoInscripcion, cancellationToken);
        var ahora = DateTime.Now;
        var (fechaRetiro, fechaEstado) = ReglasInscripcion.AplicarEstado(
            estado.Nombre,
            inscripcion.FechaInicio,
            inscripcion.FechaRetiro,
            ahora);

        inscripcion.IdEstadoInscripcion = estado.IdEstadoInscripcion;
        inscripcion.EsOficial = datos.EsOficial;
        inscripcion.Observacion = NormalizarOpcional(datos.Observacion, 500, "observación");
        inscripcion.FechaRetiro = fechaRetiro;
        inscripcion.FechaEstado = fechaEstado;

        await _contexto.SaveChangesAsync(cancellationToken);

        return await ConstruirListadoAsync(inscripcion.IdCursoOfertado, cancellationToken);
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

    private async Task<CursoInscripcionesDto> ConstruirListadoAsync(
        int idCursoOfertado,
        CancellationToken cancellationToken)
    {
        var estados = await _contexto.EstadosInscripcion
            .AsNoTracking()
            .Where(e => e.Activo)
            .OrderBy(e => e.IdEstadoInscripcion)
            .Select(e => new EstadoInscripcionDto(e.IdEstadoInscripcion, e.Nombre))
            .ToListAsync(cancellationToken);

        var filas = await _contexto.Inscripciones
            .AsNoTracking()
            .Where(i => i.IdCursoOfertado == idCursoOfertado)
            .OrderBy(i => i.Estudiante.Nombre)
            .Select(i => new InscripcionFilaDto(
                i.IdInscripcion,
                i.IdEstudiante,
                i.Estudiante.Carnet,
                i.Estudiante.Nombre,
                i.Estudiante.Sexo,
                i.IdEstadoInscripcion,
                i.EstadoInscripcion.Nombre,
                i.EsOficial,
                i.FechaInicio,
                i.FechaRetiro,
                i.Observacion))
            .ToListAsync(cancellationToken);

        return new CursoInscripcionesDto(idCursoOfertado, estados, filas);
    }

    private async Task<Infraestructura.Entidades.Cat.EstadoInscripcion> ResolverEstadoAltaAsync(
        int? idEstadoInscripcion,
        CancellationToken cancellationToken)
    {
        if (idEstadoInscripcion.HasValue)
        {
            return await ObtenerEstadoActivoAsync(idEstadoInscripcion.Value, cancellationToken);
        }

        var activa = await _contexto.EstadosInscripcion
            .FirstOrDefaultAsync(
                e => e.Activo && e.Nombre == ReglasInscripcion.Activa,
                cancellationToken);

        if (activa is null)
        {
            throw new ReglaNegocioException("No existe el estado de inscripción Activa.");
        }

        return activa;
    }

    private async Task<Infraestructura.Entidades.Cat.EstadoInscripcion> ObtenerEstadoActivoAsync(
        int idEstadoInscripcion,
        CancellationToken cancellationToken)
    {
        var estado = await _contexto.EstadosInscripcion
            .FirstOrDefaultAsync(
                e => e.IdEstadoInscripcion == idEstadoInscripcion && e.Activo,
                cancellationToken);

        if (estado is null)
        {
            throw new ReglaNegocioException(
                $"El estado de inscripción {idEstadoInscripcion} no es válido.");
        }

        return estado;
    }

    private static string? NormalizarOpcional(string? valor, int maximo, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        var texto = valor.Trim();
        if (texto.Length > maximo)
        {
            throw new ReglaNegocioException(
                $"El {campo} no puede superar {maximo} caracteres.");
        }

        return texto;
    }
}
