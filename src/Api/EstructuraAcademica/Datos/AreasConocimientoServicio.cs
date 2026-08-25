using GestionAcademica.Api.Comun;
using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.EstructuraAcademica.Datos;

public class AreasConocimientoServicio : IAreasConocimientoServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public AreasConocimientoServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<AreaConocimientoDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.AreasConocimiento
            .AsNoTracking()
            .OrderBy(a => a.Nombre)
            .Select(a => new AreaConocimientoDto(
                a.IdAreaConocimiento,
                a.Nombre,
                a.Descripcion,
                a.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<AreaConocimientoDto> ObtenerAsync(int idAreaConocimiento, CancellationToken cancellationToken)
    {
        var area = await _contexto.AreasConocimiento
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAreaConocimiento == idAreaConocimiento, cancellationToken);

        if (area is null)
        {
            throw new KeyNotFoundException($"No existe el área de conocimiento {idAreaConocimiento}.");
        }

        return Mapear(area);
    }

    public async Task<AreaConocimientoDto> CrearAsync(
        AreaConocimientoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        await AsegurarNombreLibreAsync(nombre, excluirId: null, cancellationToken);

        var area = new AreaConocimiento
        {
            Nombre = nombre,
            Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción"),
            Activo = datos.Activo
        };

        _contexto.AreasConocimiento.Add(area);
        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(area);
    }

    public async Task<AreaConocimientoDto> EditarAsync(
        int idAreaConocimiento,
        AreaConocimientoEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var area = await _contexto.AreasConocimiento
            .FirstOrDefaultAsync(a => a.IdAreaConocimiento == idAreaConocimiento, cancellationToken);

        if (area is null)
        {
            throw new KeyNotFoundException($"No existe el área de conocimiento {idAreaConocimiento}.");
        }

        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        await AsegurarNombreLibreAsync(nombre, idAreaConocimiento, cancellationToken);

        area.Nombre = nombre;
        area.Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción");
        area.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(area);
    }

    private async Task AsegurarNombreLibreAsync(string nombre, int? excluirId, CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.AreasConocimiento
            .AsNoTracking()
            .AnyAsync(
                a => a.Nombre == nombre && a.IdAreaConocimiento != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException($"Ya existe un área de conocimiento con el nombre {nombre}.");
        }
    }

    private static AreaConocimientoDto Mapear(AreaConocimiento area)
    {
        return new AreaConocimientoDto(
            area.IdAreaConocimiento,
            area.Nombre,
            area.Descripcion,
            area.Activo);
    }
}
