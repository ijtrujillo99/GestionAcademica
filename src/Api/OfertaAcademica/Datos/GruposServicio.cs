using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.OfertaAcademica.Datos;

public class GruposServicio : IGruposServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public GruposServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<GrupoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.Grupos
            .AsNoTracking()
            .OrderBy(g => g.CodigoGrupo)
            .Select(g => new GrupoDto(
                g.IdGrupo,
                g.IdPeriodo,
                g.IdCarrera,
                g.CodigoGrupo,
                g.Nombre,
                g.Descripcion,
                g.Activo,
                g.Carrera.Nombre,
                g.Periodo.Anio,
                g.Periodo.Semestre))
            .ToListAsync(cancellationToken);
    }

    public async Task<GrupoDto> ObtenerAsync(int idGrupo, CancellationToken cancellationToken)
    {
        var grupo = await _contexto.Grupos
            .AsNoTracking()
            .Where(g => g.IdGrupo == idGrupo)
            .Select(g => new GrupoDto(
                g.IdGrupo,
                g.IdPeriodo,
                g.IdCarrera,
                g.CodigoGrupo,
                g.Nombre,
                g.Descripcion,
                g.Activo,
                g.Carrera.Nombre,
                g.Periodo.Anio,
                g.Periodo.Semestre))
            .FirstOrDefaultAsync(cancellationToken);

        if (grupo is null)
        {
            throw new KeyNotFoundException($"No existe el grupo {idGrupo}.");
        }

        return grupo;
    }

    public async Task<GrupoDto> CrearAsync(GrupoEscrituraDto datos, CancellationToken cancellationToken)
    {
        await AsegurarPeriodoActivoAsync(datos.IdPeriodo, cancellationToken);
        await AsegurarCarreraActivaAsync(datos.IdCarrera, cancellationToken);
        var codigo = TextoNormalizado.Requerido(datos.CodigoGrupo, 30, "código de grupo");
        await AsegurarCodigoLibreAsync(datos.IdPeriodo, codigo, excluirId: null, cancellationToken);

        var grupo = new Grupo
        {
            IdPeriodo = datos.IdPeriodo,
            IdCarrera = datos.IdCarrera,
            CodigoGrupo = codigo,
            Nombre = TextoNormalizado.Opcional(datos.Nombre, 100, "nombre"),
            Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción"),
            Activo = datos.Activo
        };

        _contexto.Grupos.Add(grupo);
        await _contexto.SaveChangesAsync(cancellationToken);
        return await ObtenerAsync(grupo.IdGrupo, cancellationToken);
    }

    public async Task<GrupoDto> EditarAsync(int idGrupo, GrupoEscrituraDto datos, CancellationToken cancellationToken)
    {
        var grupo = await _contexto.Grupos
            .FirstOrDefaultAsync(g => g.IdGrupo == idGrupo, cancellationToken);

        if (grupo is null)
        {
            throw new KeyNotFoundException($"No existe el grupo {idGrupo}.");
        }

        await AsegurarPeriodoActivoAsync(datos.IdPeriodo, cancellationToken);
        await AsegurarCarreraActivaAsync(datos.IdCarrera, cancellationToken);
        var codigo = TextoNormalizado.Requerido(datos.CodigoGrupo, 30, "código de grupo");
        await AsegurarCodigoLibreAsync(datos.IdPeriodo, codigo, idGrupo, cancellationToken);

        grupo.IdPeriodo = datos.IdPeriodo;
        grupo.IdCarrera = datos.IdCarrera;
        grupo.CodigoGrupo = codigo;
        grupo.Nombre = TextoNormalizado.Opcional(datos.Nombre, 100, "nombre");
        grupo.Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción");
        grupo.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return await ObtenerAsync(idGrupo, cancellationToken);
    }

    private async Task AsegurarPeriodoActivoAsync(int idPeriodo, CancellationToken cancellationToken)
    {
        var periodo = await _contexto.PeriodosAcademicos
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPeriodo == idPeriodo, cancellationToken);

        if (periodo is null)
        {
            throw new KeyNotFoundException($"No existe el período académico {idPeriodo}.");
        }

        if (!periodo.Activo)
        {
            throw new ReglaNegocioException("El período académico no está activo.");
        }
    }

    private async Task AsegurarCarreraActivaAsync(int idCarrera, CancellationToken cancellationToken)
    {
        var carrera = await _contexto.Carreras
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCarrera == idCarrera, cancellationToken);

        if (carrera is null)
        {
            throw new KeyNotFoundException($"No existe la carrera {idCarrera}.");
        }

        if (!carrera.Activo)
        {
            throw new ReglaNegocioException("La carrera no está activa.");
        }
    }

    private async Task AsegurarCodigoLibreAsync(
        int idPeriodo,
        string codigo,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.Grupos
            .AsNoTracking()
            .AnyAsync(
                g => g.IdPeriodo == idPeriodo
                     && g.CodigoGrupo == codigo
                     && g.IdGrupo != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException(
                $"Ya existe el grupo {codigo} en este período.");
        }
    }
}
