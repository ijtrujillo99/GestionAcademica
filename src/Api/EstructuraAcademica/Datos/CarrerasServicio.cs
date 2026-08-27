using GestionAcademica.Api.Comun;
using GestionAcademica.Api.EstructuraAcademica.Aplicacion;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.EstructuraAcademica.Datos;

public class CarrerasServicio : ICarrerasServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public CarrerasServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<CarreraDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.Carreras
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .Select(c => new CarreraDto(
                c.IdCarrera,
                c.IdAreaConocimiento,
                c.AreaConocimiento.Nombre,
                c.Nombre,
                c.Descripcion,
                c.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<CarreraDto> ObtenerAsync(int idCarrera, CancellationToken cancellationToken)
    {
        var carrera = await _contexto.Carreras
            .AsNoTracking()
            .Where(c => c.IdCarrera == idCarrera)
            .Select(c => new CarreraDto(
                c.IdCarrera,
                c.IdAreaConocimiento,
                c.AreaConocimiento.Nombre,
                c.Nombre,
                c.Descripcion,
                c.Activo))
            .FirstOrDefaultAsync(cancellationToken);

        if (carrera is null)
        {
            throw new KeyNotFoundException($"No existe la carrera {idCarrera}.");
        }

        return carrera;
    }

    public async Task<CarreraDto> CrearAsync(CarreraEscrituraDto datos, CancellationToken cancellationToken)
    {
        await AsegurarAreaActivaAsync(datos.IdAreaConocimiento, cancellationToken);
        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        await AsegurarNombreLibreAsync(datos.IdAreaConocimiento, nombre, excluirId: null, cancellationToken);

        var carrera = new Carrera
        {
            IdAreaConocimiento = datos.IdAreaConocimiento,
            Nombre = nombre,
            Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción"),
            Activo = datos.Activo
        };

        _contexto.Carreras.Add(carrera);
        await _contexto.SaveChangesAsync(cancellationToken);
        return await ObtenerAsync(carrera.IdCarrera, cancellationToken);
    }

    public async Task<CarreraDto> EditarAsync(
        int idCarrera,
        CarreraEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var carrera = await _contexto.Carreras
            .FirstOrDefaultAsync(c => c.IdCarrera == idCarrera, cancellationToken);

        if (carrera is null)
        {
            throw new KeyNotFoundException($"No existe la carrera {idCarrera}.");
        }

        await AsegurarAreaActivaAsync(datos.IdAreaConocimiento, cancellationToken);
        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        await AsegurarNombreLibreAsync(datos.IdAreaConocimiento, nombre, idCarrera, cancellationToken);

        carrera.IdAreaConocimiento = datos.IdAreaConocimiento;
        carrera.Nombre = nombre;
        carrera.Descripcion = TextoNormalizado.Opcional(datos.Descripcion, 300, "descripción");
        carrera.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return await ObtenerAsync(idCarrera, cancellationToken);
    }

    private async Task AsegurarAreaActivaAsync(int idAreaConocimiento, CancellationToken cancellationToken)
    {
        var area = await _contexto.AreasConocimiento
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAreaConocimiento == idAreaConocimiento, cancellationToken);

        if (area is null)
        {
            throw new KeyNotFoundException($"No existe el área de conocimiento {idAreaConocimiento}.");
        }

        if (!area.Activo)
        {
            throw new ReglaNegocioException("El área de conocimiento no está activa.");
        }
    }

    private async Task AsegurarNombreLibreAsync(
        int idAreaConocimiento,
        string nombre,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.Carreras
            .AsNoTracking()
            .AnyAsync(
                c => c.IdAreaConocimiento == idAreaConocimiento
                     && c.Nombre == nombre
                     && c.IdCarrera != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException(
                $"Ya existe una carrera con el nombre {nombre} en esta área.");
        }
    }
}
