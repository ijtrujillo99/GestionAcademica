using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Comun.Paginacion;

public static class PaginacionQueryable
{
    public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntidad, TDto>(
        this IQueryable<TEntidad> consulta,
        PaginacionConsulta paginacion,
        Func<IQueryable<TEntidad>, IOrderedQueryable<TEntidad>> ordenar,
        Expression<Func<TEntidad, TDto>> proyeccion,
        CancellationToken cancellationToken)
    {
        var totalItems = await consulta.CountAsync(cancellationToken);

        var items = await ordenar(consulta)
            .Skip(paginacion.Saltar)
            .Take(paginacion.PageSize)
            .Select(proyeccion)
            .ToListAsync(cancellationToken);

        return PagedResult<TDto>.Crear(items, paginacion.Page, paginacion.PageSize, totalItems);
    }
}
