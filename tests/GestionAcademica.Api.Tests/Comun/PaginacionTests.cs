using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Comun.Paginacion;

namespace GestionAcademica.Api.Tests.Comun;

public sealed class PaginacionTests
{
    [Fact]
    public void Paginar_CuandoHay36RegistrosEnPagina1De10_Devuelve10ItemsY4Paginas()
    {
        var resultado = Paginar(Fuente(36), page: 1, pageSize: 10);

        Assert.Equal(10, resultado.Items.Count);
        Assert.Equal(1, resultado.Page);
        Assert.Equal(10, resultado.PageSize);
        Assert.Equal(36, resultado.TotalItems);
        Assert.Equal(4, resultado.TotalPages);
        Assert.Equal(Enumerable.Range(1, 10), resultado.Items);
    }

    [Fact]
    public void Paginar_CuandoEsPagina2De10_Devuelve10Items()
    {
        var resultado = Paginar(Fuente(36), page: 2, pageSize: 10);

        Assert.Equal(10, resultado.Items.Count);
        Assert.Equal(Enumerable.Range(11, 10), resultado.Items);
    }

    [Fact]
    public void Paginar_CuandoEsLaUltimaPagina_DevuelveElRemanente()
    {
        var resultado = Paginar(Fuente(36), page: 4, pageSize: 10);

        Assert.Equal(6, resultado.Items.Count);
        Assert.Equal(Enumerable.Range(31, 6), resultado.Items);
        Assert.Equal(4, resultado.TotalPages);
    }

    [Fact]
    public void Paginar_CuandoLaPaginaExcedeElTotal_DevuelveItemsVacios()
    {
        var resultado = Paginar(Fuente(36), page: 5, pageSize: 10);

        Assert.Empty(resultado.Items);
        Assert.Equal(5, resultado.Page);
        Assert.Equal(10, resultado.PageSize);
        Assert.Equal(36, resultado.TotalItems);
        Assert.Equal(4, resultado.TotalPages);
    }

    [Fact]
    public void Paginar_CuandoElTamanoEs25_Devuelve2Paginas()
    {
        var resultado = Paginar(Fuente(36), page: 1, pageSize: 25);

        Assert.Equal(25, resultado.Items.Count);
        Assert.Equal(2, resultado.TotalPages);
    }

    [Fact]
    public void Crear_CuandoElTamanoEs100_EsValido()
    {
        var paginacion = PaginacionConsulta.Crear(1, 100);

        Assert.Equal(100, paginacion.PageSize);
    }

    [Fact]
    public void Crear_CuandoElTamanoEs101_Lanza()
    {
        var error = Assert.Throws<ReglaNegocioException>(() =>
            PaginacionConsulta.Crear(1, 101));

        Assert.Contains("1 y 100", error.Message);
    }

    [Fact]
    public void Crear_CuandoLaPaginaEs0_Lanza()
    {
        Assert.Throws<ReglaNegocioException>(() => PaginacionConsulta.Crear(0, 10));
    }

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -5)]
    public void Crear_CuandoLosParametrosSonInvalidos_Lanza(int page, int pageSize)
    {
        Assert.Throws<ReglaNegocioException>(() => PaginacionConsulta.Crear(page, pageSize));
    }

    [Fact]
    public void Paginar_CuandoHayBusqueda_ElTotalCorrespondeAlFiltro()
    {
        var filtrados = Fuente(36).Where(n => n % 10 == 0).ToList();
        var resultado = Paginar(filtrados, page: 1, pageSize: 10);

        Assert.Equal(3, resultado.TotalItems);
        Assert.Equal(1, resultado.TotalPages);
        Assert.Equal(new[] { 10, 20, 30 }, resultado.Items);
    }

    [Fact]
    public void CalcularTotalPaginas_CuandoNoHayItems_EsCero()
    {
        var resultado = Paginar(Array.Empty<int>(), page: 1, pageSize: 10);

        Assert.Empty(resultado.Items);
        Assert.Equal(0, resultado.TotalItems);
        Assert.Equal(0, resultado.TotalPages);
    }

    [Theory]
    [InlineData(36, 10, 4)]
    [InlineData(36, 25, 2)]
    [InlineData(36, 50, 1)]
    [InlineData(0, 10, 0)]
    public void CalcularTotalPaginas_UsaTecho(int totalItems, int pageSize, int esperado)
    {
        Assert.Equal(esperado, PagedResult<int>.CalcularTotalPaginas(totalItems, pageSize));
    }

    private static PagedResult<int> Paginar(IReadOnlyList<int> fuente, int page, int pageSize)
    {
        var paginacion = PaginacionConsulta.Crear(page, pageSize);
        var items = fuente
            .Skip(paginacion.Saltar)
            .Take(paginacion.PageSize)
            .ToList();

        return PagedResult<int>.Crear(items, paginacion.Page, paginacion.PageSize, fuente.Count);
    }

    private static IReadOnlyList<int> Fuente(int total)
    {
        return Enumerable.Range(1, total).ToList();
    }
}
