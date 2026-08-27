using GestionAcademica.Api.Comun;
using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.EstructuraAcademica.Datos;

public class PlanesEstudioServicio : IPlanesEstudioServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public PlanesEstudioServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<PlanEstudioDto>> ListarPorCarreraAsync(
        int idCarrera,
        CancellationToken cancellationToken)
    {
        await AsegurarCarreraAsync(idCarrera, exigirActiva: false, cancellationToken);

        return await _contexto.PlanesEstudio
            .AsNoTracking()
            .Where(p => p.IdCarrera == idCarrera)
            .OrderBy(p => p.AnioInicio)
            .Select(p => new PlanEstudioDto(
                p.IdPlanEstudio,
                p.IdCarrera,
                p.Nombre,
                p.AnioInicio,
                p.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<PlanEstudioDto> ObtenerAsync(int idPlanEstudio, CancellationToken cancellationToken)
    {
        var plan = await _contexto.PlanesEstudio
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPlanEstudio == idPlanEstudio, cancellationToken);

        if (plan is null)
        {
            throw new KeyNotFoundException($"No existe el plan de estudio {idPlanEstudio}.");
        }

        return Mapear(plan);
    }

    public async Task<PlanEstudioDto> CrearAsync(
        int idCarrera,
        PlanEstudioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        await AsegurarCarreraAsync(idCarrera, exigirActiva: true, cancellationToken);
        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        ValidarAnio(datos.AnioInicio);
        await AsegurarAnioLibreAsync(idCarrera, datos.AnioInicio, excluirId: null, cancellationToken);

        var plan = new PlanEstudio
        {
            IdCarrera = idCarrera,
            Nombre = nombre,
            AnioInicio = datos.AnioInicio,
            Activo = datos.Activo
        };

        _contexto.PlanesEstudio.Add(plan);
        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(plan);
    }

    public async Task<PlanEstudioDto> EditarAsync(
        int idPlanEstudio,
        PlanEstudioEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var plan = await _contexto.PlanesEstudio
            .FirstOrDefaultAsync(p => p.IdPlanEstudio == idPlanEstudio, cancellationToken);

        if (plan is null)
        {
            throw new KeyNotFoundException($"No existe el plan de estudio {idPlanEstudio}.");
        }

        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        ValidarAnio(datos.AnioInicio);
        await AsegurarAnioLibreAsync(plan.IdCarrera, datos.AnioInicio, idPlanEstudio, cancellationToken);

        plan.Nombre = nombre;
        plan.AnioInicio = datos.AnioInicio;
        plan.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(plan);
    }

    public async Task<IReadOnlyList<PlanDetalleDto>> ListarAsignaturasAsync(
        int idPlanEstudio,
        CancellationToken cancellationToken)
    {
        await ObtenerAsync(idPlanEstudio, cancellationToken);
        return await ListarDetallesAsync(idPlanEstudio, cancellationToken);
    }

    public async Task<IReadOnlyList<PlanDetalleDto>> AgregarAsignaturaAsync(
        int idPlanEstudio,
        PlanDetalleEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var plan = await _contexto.PlanesEstudio
            .FirstOrDefaultAsync(p => p.IdPlanEstudio == idPlanEstudio, cancellationToken);

        if (plan is null)
        {
            throw new KeyNotFoundException($"No existe el plan de estudio {idPlanEstudio}.");
        }

        await AsegurarAsignaturaActivaAsync(datos.IdAsignatura, cancellationToken);
        ValidarDetalle(datos);
        await AsegurarAsignaturaLibreEnPlanAsync(
            idPlanEstudio,
            datos.IdAsignatura,
            excluirId: null,
            cancellationToken);

        _contexto.PlanesEstudioDetalle.Add(new PlanEstudioDetalle
        {
            IdPlanEstudio = idPlanEstudio,
            IdAsignatura = datos.IdAsignatura,
            SemestrePlan = datos.SemestrePlan,
            Creditos = datos.Creditos,
            HorasTeoria = datos.HorasTeoria,
            HorasPractica = datos.HorasPractica,
            Orden = datos.Orden,
            EsElectiva = datos.EsElectiva
        });

        await _contexto.SaveChangesAsync(cancellationToken);
        return await ListarDetallesAsync(idPlanEstudio, cancellationToken);
    }

    public async Task<IReadOnlyList<PlanDetalleDto>> EditarAsignaturaAsync(
        int idPlanDetalle,
        PlanDetalleEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var detalle = await _contexto.PlanesEstudioDetalle
            .FirstOrDefaultAsync(d => d.IdPlanDetalle == idPlanDetalle, cancellationToken);

        if (detalle is null)
        {
            throw new KeyNotFoundException($"No existe el detalle de plan {idPlanDetalle}.");
        }

        await AsegurarAsignaturaActivaAsync(datos.IdAsignatura, cancellationToken);
        ValidarDetalle(datos);
        await AsegurarAsignaturaLibreEnPlanAsync(
            detalle.IdPlanEstudio,
            datos.IdAsignatura,
            idPlanDetalle,
            cancellationToken);

        detalle.IdAsignatura = datos.IdAsignatura;
        detalle.SemestrePlan = datos.SemestrePlan;
        detalle.Creditos = datos.Creditos;
        detalle.HorasTeoria = datos.HorasTeoria;
        detalle.HorasPractica = datos.HorasPractica;
        detalle.Orden = datos.Orden;
        detalle.EsElectiva = datos.EsElectiva;

        await _contexto.SaveChangesAsync(cancellationToken);
        return await ListarDetallesAsync(detalle.IdPlanEstudio, cancellationToken);
    }

    private async Task<IReadOnlyList<PlanDetalleDto>> ListarDetallesAsync(
        int idPlanEstudio,
        CancellationToken cancellationToken)
    {
        return await _contexto.PlanesEstudioDetalle
            .AsNoTracking()
            .Where(d => d.IdPlanEstudio == idPlanEstudio)
            .OrderBy(d => d.SemestrePlan)
            .ThenBy(d => d.Orden)
            .ThenBy(d => d.Asignatura.Nombre)
            .Select(d => new PlanDetalleDto(
                d.IdPlanDetalle,
                d.IdPlanEstudio,
                d.IdAsignatura,
                d.Asignatura.Codigo,
                d.Asignatura.Nombre,
                d.SemestrePlan,
                d.Creditos,
                d.HorasTeoria,
                d.HorasPractica,
                d.Orden,
                d.EsElectiva))
            .ToListAsync(cancellationToken);
    }

    private async Task AsegurarCarreraAsync(int idCarrera, bool exigirActiva, CancellationToken cancellationToken)
    {
        var carrera = await _contexto.Carreras
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCarrera == idCarrera, cancellationToken);

        if (carrera is null)
        {
            throw new KeyNotFoundException($"No existe la carrera {idCarrera}.");
        }

        if (exigirActiva && !carrera.Activo)
        {
            throw new ReglaNegocioException("La carrera no está activa.");
        }
    }

    private async Task AsegurarAsignaturaActivaAsync(int idAsignatura, CancellationToken cancellationToken)
    {
        var asignatura = await _contexto.Asignaturas
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAsignatura == idAsignatura, cancellationToken);

        if (asignatura is null)
        {
            throw new KeyNotFoundException($"No existe la asignatura {idAsignatura}.");
        }

        if (!asignatura.Activo)
        {
            throw new ReglaNegocioException("La asignatura no está activa.");
        }
    }

    private async Task AsegurarAnioLibreAsync(
        int idCarrera,
        short anioInicio,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.PlanesEstudio
            .AsNoTracking()
            .AnyAsync(
                p => p.IdCarrera == idCarrera
                     && p.AnioInicio == anioInicio
                     && p.IdPlanEstudio != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException(
                $"Ya existe un plan de estudio con año de inicio {anioInicio} en esta carrera.");
        }
    }

    private async Task AsegurarAsignaturaLibreEnPlanAsync(
        int idPlanEstudio,
        int idAsignatura,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.PlanesEstudioDetalle
            .AsNoTracking()
            .AnyAsync(
                d => d.IdPlanEstudio == idPlanEstudio
                     && d.IdAsignatura == idAsignatura
                     && d.IdPlanDetalle != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException("La asignatura ya está en este plan de estudio.");
        }
    }

    private static void ValidarAnio(short anioInicio)
    {
        if (anioInicio < 2000)
        {
            throw new ReglaNegocioException("El año de inicio debe ser 2000 o posterior.");
        }
    }

    private static void ValidarDetalle(PlanDetalleEscrituraDto datos)
    {
        if (datos.SemestrePlan is < 1 or > 20)
        {
            throw new ReglaNegocioException("El semestre del plan debe estar entre 1 y 20.");
        }

        if (datos.Creditos == 0)
        {
            throw new ReglaNegocioException("Los créditos deben ser mayores que 0.");
        }

        if (datos.HorasTeoria < 0 || datos.HorasPractica < 0)
        {
            throw new ReglaNegocioException("Las horas no pueden ser negativas.");
        }
    }

    private static PlanEstudioDto Mapear(PlanEstudio plan)
    {
        return new PlanEstudioDto(
            plan.IdPlanEstudio,
            plan.IdCarrera,
            plan.Nombre,
            plan.AnioInicio,
            plan.Activo);
    }
}
