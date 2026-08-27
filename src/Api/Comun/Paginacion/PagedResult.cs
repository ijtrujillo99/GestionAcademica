namespace GestionAcademica.Api.Comun.Paginacion;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages)
{
    public static PagedResult<T> Crear(
        IReadOnlyList<T> items,
        int page,
        int pageSize,
        int totalItems)
    {
        return new PagedResult<T>(
            items,
            page,
            pageSize,
            totalItems,
            CalcularTotalPaginas(totalItems, pageSize));
    }

    public static int CalcularTotalPaginas(int totalItems, int pageSize)
    {
        if (totalItems <= 0 || pageSize <= 0)
        {
            return 0;
        }

        return (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
