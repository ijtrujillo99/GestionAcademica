using GestionAcademica.Api.Comun;
using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.EstructuraAcademica.Datos;

public class AsignaturasServicio : IAsignaturasServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public AsignaturasServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<AsignaturaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.Asignaturas
            .AsNoTracking()
            .OrderBy(a => a.Nombre)
            .Select(a => new AsignaturaDto(
                a.IdAsignatura,
                a.Codigo,
                a.Nombre,
                a.Descripcion,
                a.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<AsignaturaDto> ObtenerAsync(int idAsignatura, CancellationToken cancellationToken)
    {
        var asignatura = await _contexto.Asignaturas
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAsignatura == idAsignatura, cancellationToken);

        if (asignatura is null)
        {
            throw new KeyNotFoundException($"No existe la asignatura {idAsignatura}.");
        }

        return Mapear(asignatura);
    }

    public async Task<AsignaturaDto> CrearAsync(AsignaturaEscrituraDto datos, CancellationToken cancellationToken)
    {
        var codigo = TextoNormalizado.Requerido(datos.Codigo, 30, "código");
        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        await AsegurarCodigoLibreAsync(codigo, excluirId: null, cancellationToken);

        var asignatura = new Asignatura
        {
            Codigo = codigo,
            Nombre = nombre,
            Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción"),
            Activo = datos.Activo
        };

        _contexto.Asignaturas.Add(asignatura);
        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(asignatura);
    }

    public async Task<AsignaturaDto> EditarAsync(
        int idAsignatura,
        AsignaturaEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var asignatura = await _contexto.Asignaturas
            .FirstOrDefaultAsync(a => a.IdAsignatura == idAsignatura, cancellationToken);

        if (asignatura is null)
        {
            throw new KeyNotFoundException($"No existe la asignatura {idAsignatura}.");
        }

        var codigo = TextoNormalizado.Requerido(datos.Codigo, 30, "código");
        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        await AsegurarCodigoLibreAsync(codigo, idAsignatura, cancellationToken);

        asignatura.Codigo = codigo;
        asignatura.Nombre = nombre;
        asignatura.Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción");
        asignatura.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(asignatura);
    }

    private async Task AsegurarCodigoLibreAsync(string codigo, int? excluirId, CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.Asignaturas
            .AsNoTracking()
            .AnyAsync(
                a => a.Codigo == codigo && a.IdAsignatura != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException($"Ya existe una asignatura con el código {codigo}.");
        }
    }

    private static AsignaturaDto Mapear(Asignatura asignatura)
    {
        return new AsignaturaDto(
            asignatura.IdAsignatura,
            asignatura.Codigo,
            asignatura.Nombre,
            asignatura.Descripcion,
            asignatura.Activo);
    }
}
