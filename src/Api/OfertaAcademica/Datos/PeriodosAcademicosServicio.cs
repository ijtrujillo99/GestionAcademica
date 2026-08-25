using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.OfertaAcademica.Datos;

public class PeriodosAcademicosServicio : IPeriodosAcademicosServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public PeriodosAcademicosServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<PeriodoAcademicoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.PeriodosAcademicos
            .AsNoTracking()
            .OrderByDescending(p => p.Anio)
            .ThenByDescending(p => p.Semestre)
            .Select(p => new PeriodoAcademicoDto(
                p.IdPeriodo,
                p.Anio,
                p.Semestre,
                p.FechaInicio,
                p.FechaFin,
                p.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<PeriodoAcademicoDto> ObtenerAsync(int idPeriodo, CancellationToken cancellationToken)
    {
        var periodo = await _contexto.PeriodosAcademicos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPeriodo == idPeriodo, cancellationToken);

        if (periodo is null)
        {
            throw new KeyNotFoundException($"No existe el período académico {idPeriodo}.");
        }

        return Mapear(periodo);
    }

    public async Task<PeriodoAcademicoDto> CrearAsync(
        PeriodoAcademicoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        Validar(datos);
        await AsegurarPeriodoLibreAsync(datos.Anio, datos.Semestre, excluirId: null, cancellationToken);

        var periodo = new PeriodoAcademico
        {
            Anio = datos.Anio,
            Semestre = datos.Semestre,
            FechaInicio = datos.FechaInicio.Date,
            FechaFin = datos.FechaFin.Date,
            Activo = datos.Activo
        };

        _contexto.PeriodosAcademicos.Add(periodo);
        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(periodo);
    }

    public async Task<PeriodoAcademicoDto> EditarAsync(
        int idPeriodo,
        PeriodoAcademicoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var periodo = await _contexto.PeriodosAcademicos
            .FirstOrDefaultAsync(p => p.IdPeriodo == idPeriodo, cancellationToken);

        if (periodo is null)
        {
            throw new KeyNotFoundException($"No existe el período académico {idPeriodo}.");
        }

        Validar(datos);
        await AsegurarPeriodoLibreAsync(datos.Anio, datos.Semestre, idPeriodo, cancellationToken);

        periodo.Anio = datos.Anio;
        periodo.Semestre = datos.Semestre;
        periodo.FechaInicio = datos.FechaInicio.Date;
        periodo.FechaFin = datos.FechaFin.Date;
        periodo.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(periodo);
    }

    private async Task AsegurarPeriodoLibreAsync(
        short anio,
        byte semestre,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.PeriodosAcademicos
            .AsNoTracking()
            .AnyAsync(
                p => p.Anio == anio && p.Semestre == semestre && p.IdPeriodo != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException($"Ya existe el período {anio}-{semestre}.");
        }
    }

    private static void Validar(PeriodoAcademicoEscrituraDto datos)
    {
        if (datos.Semestre is not (1 or 2))
        {
            throw new ReglaNegocioException("El semestre debe ser 1 o 2.");
        }

        if (datos.FechaFin.Date < datos.FechaInicio.Date)
        {
            throw new ReglaNegocioException("La fecha de fin no puede ser anterior a la de inicio.");
        }
    }

    private static PeriodoAcademicoDto Mapear(PeriodoAcademico periodo)
    {
        return new PeriodoAcademicoDto(
            periodo.IdPeriodo,
            periodo.Anio,
            periodo.Semestre,
            periodo.FechaInicio,
            periodo.FechaFin,
            periodo.Activo);
    }
}
