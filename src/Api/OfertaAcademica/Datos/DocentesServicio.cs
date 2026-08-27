using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using GestionAcademica.Api.OfertaAcademica.Aplicacion;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.OfertaAcademica.Datos;

public class DocentesServicio : IDocentesServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public DocentesServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<DocenteDto>> ListarAsync(CancellationToken cancellationToken)
    {
        return await _contexto.Docentes
            .AsNoTracking()
            .OrderBy(d => d.Nombre)
            .Select(d => new DocenteDto(
                d.IdDocente,
                d.Nombre,
                d.Correo,
                d.Telefono,
                d.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<DocenteDto> ObtenerAsync(int idDocente, CancellationToken cancellationToken)
    {
        var docente = await _contexto.Docentes
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.IdDocente == idDocente, cancellationToken);

        if (docente is null)
        {
            throw new KeyNotFoundException($"No existe el docente {idDocente}.");
        }

        return Mapear(docente);
    }

    public async Task<DocenteDto> CrearAsync(DocenteEscrituraDto datos, CancellationToken cancellationToken)
    {
        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        var correo = TextoNormalizado.Requerido(datos.Correo, 150, "correo");
        await AsegurarCorreoLibreAsync(correo, excluirId: null, cancellationToken);

        var docente = new Docente
        {
            Nombre = nombre,
            Correo = correo,
            Telefono = TextoNormalizado.Opcional(datos.Telefono, 30, "teléfono"),
            Activo = datos.Activo
        };

        _contexto.Docentes.Add(docente);
        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(docente);
    }

    public async Task<DocenteDto> EditarAsync(int idDocente, DocenteEscrituraDto datos, CancellationToken cancellationToken)
    {
        var docente = await _contexto.Docentes
            .FirstOrDefaultAsync(d => d.IdDocente == idDocente, cancellationToken);

        if (docente is null)
        {
            throw new KeyNotFoundException($"No existe el docente {idDocente}.");
        }

        var nombre = TextoNormalizado.Requerido(datos.Nombre, 150, "nombre");
        var correo = TextoNormalizado.Requerido(datos.Correo, 150, "correo");
        await AsegurarCorreoLibreAsync(correo, idDocente, cancellationToken);

        docente.Nombre = nombre;
        docente.Correo = correo;
        docente.Telefono = TextoNormalizado.Opcional(datos.Telefono, 30, "teléfono");
        docente.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);
        return Mapear(docente);
    }

    private async Task AsegurarCorreoLibreAsync(string correo, int? excluirId, CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.Docentes
            .AsNoTracking()
            .AnyAsync(d => d.Correo == correo && d.IdDocente != (excluirId ?? 0), cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException($"Ya existe un docente con el correo {correo}.");
        }
    }

    private static DocenteDto Mapear(Docente docente)
    {
        return new DocenteDto(
            docente.IdDocente,
            docente.Nombre,
            docente.Correo,
            docente.Telefono,
            docente.Activo);
    }
}
