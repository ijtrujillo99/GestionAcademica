using GestionAcademica.Api.Catalogos.Aplicacion;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Catalogos.Datos;

public class CatalogosServicio : ICatalogosServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public CatalogosServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<CatalogosResumenDto> ObtenerResumenAsync(CancellationToken cancellationToken)
    {
        return new CatalogosResumenDto(
            await _contexto.EstadosInscripcion.CountAsync(cancellationToken),
            await _contexto.EstadosAsistencia.CountAsync(cancellationToken),
            await _contexto.TiposEvaluacion.CountAsync(cancellationToken));
    }
}
