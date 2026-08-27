namespace GestionAcademica.Api.Comun.Paginacion;

public sealed record PaginacionConsulta(int Page, int PageSize)
{
    public const int PaginaPredeterminada = 1;
    public const int TamanoPredeterminado = 10;
    public const int TamanoMaximo = 100;

    public int Saltar => (Page - 1) * PageSize;

    public static PaginacionConsulta Crear(int page, int pageSize)
    {
        if (page < PaginaPredeterminada)
        {
            throw new ReglaNegocioException("La página debe ser mayor o igual que 1.");
        }

        if (pageSize < 1 || pageSize > TamanoMaximo)
        {
            throw new ReglaNegocioException(
                $"El tamaño de página debe estar entre 1 y {TamanoMaximo}.");
        }

        if ((long)(page - 1) * pageSize > int.MaxValue)
        {
            throw new ReglaNegocioException("La página solicitada es demasiado grande.");
        }

        return new PaginacionConsulta(page, pageSize);
    }
}
