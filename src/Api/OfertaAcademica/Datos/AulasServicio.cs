using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.OfertaAcademica.Datos;

public class AulasServicio : IAulasServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public AulasServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<AulaDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.Aulas
            .AsNoTracking()
            .OrderBy(a => a.CodigoAula)
            .Select(a => new AulaDto(
                a.IdAula,
                a.CodigoAula,
                a.Nombre,
                a.Capacidad,
                a.Ubicacion,
                a.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<AulaDto> ObtenerAsync(int idAula, CancellationToken cancellationToken)
    {
        var aula = await _contexto.Aulas
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.IdAula == idAula, cancellationToken);

        if (aula is null)
        {
            throw new KeyNotFoundException($"No existe el aula {idAula}.");
        }

        return Mapear(aula);
    }

    public async Task<AulaDto> CrearAsync(AulaEscrituraDto datos, CancellationToken cancellationToken)
    {
        var codigo = TextoNormalizado.Requerido(datos.CodigoAula, 30, "código de aula");
        ValidarCapacidad(datos.Capacidad);
        await AsegurarCodigoLibreAsync(codigo, excluirId: null, cancellationToken);

        var aula = new Aula
        {
            CodigoAula = codigo,
            Nombre = TextoNormalizado.Opcional(datos.Nombre, 100, "nombre"),
            Capacidad = datos.Capacidad,
            Ubicacion = TextoNormalizado.Opcional(datos.Ubicacion, 150, "ubicación"),
            Activo = datos.Activo
        };

        _contexto.Aulas.Add(aula);
        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(aula);
    }

    public async Task<AulaDto> EditarAsync(int idAula, AulaEscrituraDto datos, CancellationToken cancellationToken)
    {
        var aula = await _contexto.Aulas
            .FirstOrDefaultAsync(a => a.IdAula == idAula, cancellationToken);

        if (aula is null)
        {
            throw new KeyNotFoundException($"No existe el aula {idAula}.");
        }

        var codigo = TextoNormalizado.Requerido(datos.CodigoAula, 30, "código de aula");
        ValidarCapacidad(datos.Capacidad);
        await AsegurarCodigoLibreAsync(codigo, idAula, cancellationToken);

        aula.CodigoAula = codigo;
        aula.Nombre = TextoNormalizado.Opcional(datos.Nombre, 100, "nombre");
        aula.Capacidad = datos.Capacidad;
        aula.Ubicacion = TextoNormalizado.Opcional(datos.Ubicacion, 150, "ubicación");
        aula.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(aula);
    }

    private async Task AsegurarCodigoLibreAsync(string codigo, int? excluirId, CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.Aulas
            .AsNoTracking()
            .AnyAsync(a => a.CodigoAula == codigo && a.IdAula != (excluirId ?? 0), cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException($"Ya existe un aula con el código {codigo}.");
        }
    }

    private static void ValidarCapacidad(short? capacidad)
    {
        if (capacidad is <= 0)
        {
            throw new ReglaNegocioException("La capacidad debe ser mayor que 0.");
        }
    }

    private static AulaDto Mapear(Aula aula)
    {
        return new AulaDto(
            aula.IdAula,
            aula.CodigoAula,
            aula.Nombre,
            aula.Capacidad,
            aula.Ubicacion,
            aula.Activo);
    }
}
