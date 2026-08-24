using GestionAcademica.Api.Reportes.Aplicacion;

namespace GestionAcademica.Api.Tests.Reportes;

public sealed class SituacionAcademicaServicioTests
{
    private readonly Mock<IResumenSituacionConsulta> _consulta = new();
    private readonly SituacionAcademicaServicio _sut;

    public SituacionAcademicaServicioTests()
    {
        _sut = new SituacionAcademicaServicio(
            _consulta.Object,
            new ClasificadorResultadoAcademico());
    }

    [Fact]
    public async Task ObtenerAsync_CuandoEstudianteCanceloInscripcion_DevuelveRetirado()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionRetirada,
            evaluacionesPendientes: 2,
            parcialI: 18m,
            parcialII: 0m,
            puntajeTareaCurso: null,
            idInscripcion: 6,
            idEstudiante: 10,
            carnet: "E2024-005",
            nombre: "Gabriel");
        ConfigurarConsulta(1, resumen);

        var filas = await _sut.ObtenerAsync(1, CancellationToken.None);

        var fila = Assert.Single(filas);
        Assert.Equal(ResultadoAcademico.Retirado, fila.ResultadoAcademico);
        Assert.Equal("E2024-005", fila.Carnet);
        VerificarConsulta(1);
    }

    [Fact]
    public async Task ObtenerAsync_CuandoSigueActivoConEvaluacionesPendientes_DevuelveEnCurso()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 3,
            parcialI: 22m,
            parcialII: 0m,
            puntajeTareaCurso: null);
        ConfigurarConsulta(2, resumen);

        var filas = await _sut.ObtenerAsync(2, CancellationToken.None);

        var fila = Assert.Single(filas);
        Assert.Equal(ResultadoAcademico.EnCurso, fila.ResultadoAcademico);
        Assert.Equal(3, fila.EvaluacionesPendientes);
        VerificarConsulta(2);
    }

    [Fact]
    public async Task ObtenerAsync_CuandoCierraEvaluacionesYAlcanzaSesentaConTarea_DevuelveAprobado()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 0,
            parcialI: 35m,
            parcialII: 30m,
            puntajeTareaCurso: 20m);
        ConfigurarConsulta(3, resumen);

        var filas = await _sut.ObtenerAsync(3, CancellationToken.None);

        var fila = Assert.Single(filas);
        Assert.Equal(65m, fila.NotaFinal);
        Assert.Equal(20m, fila.PuntajeTareaCurso);
        Assert.Equal(ResultadoAcademico.NotaMinimaAprobacion, fila.NotaMinimaAprobacion);
        Assert.Equal(ResultadoAcademico.Aprobado, fila.ResultadoAcademico);
        VerificarConsulta(3);
    }

    [Fact]
    public async Task ObtenerAsync_CuandoCierraEvaluacionesYNoAlcanzaSesenta_DevuelveReprobado()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 0,
            parcialI: 25m,
            parcialII: 25m,
            puntajeTareaCurso: 5m);
        ConfigurarConsulta(4, resumen);

        var filas = await _sut.ObtenerAsync(4, CancellationToken.None);

        var fila = Assert.Single(filas);
        Assert.Equal(50m, fila.NotaFinal);
        Assert.Equal(ResultadoAcademico.Reprobado, fila.ResultadoAcademico);
        VerificarConsulta(4);
    }

    [Fact]
    public async Task ObtenerAsync_CuandoAlcanzaSesentaPeroRepruebaTareaDeCurso_DevuelveNoAprobadoTareaCurso()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 0,
            parcialI: 35m,
            parcialII: 30m,
            puntajeTareaCurso: 0m);
        ConfigurarConsulta(5, resumen);

        var filas = await _sut.ObtenerAsync(5, CancellationToken.None);

        var fila = Assert.Single(filas);
        Assert.Equal(65m, fila.NotaFinal);
        Assert.Equal(0m, fila.PuntajeTareaCurso);
        Assert.Equal(ResultadoAcademico.NoAprobadoTareaCurso, fila.ResultadoAcademico);
        VerificarConsulta(5);
    }

    [Fact]
    public async Task ObtenerAsync_CuandoElCursoTieneVariosEstudiantes_ClasificaCadaUno()
    {
        var resumenes = new[]
        {
            ResumenSituacionFactory.Crear(
                estadoInscripcion: ResultadoAcademico.EstadoInscripcionRetirada,
                idInscripcion: 6,
                nombre: "Gabriel"),
            ResumenSituacionFactory.Crear(
                evaluacionesPendientes: 2,
                idInscripcion: 7,
                nombre: "Daniela"),
            ResumenSituacionFactory.Crear(
                evaluacionesPendientes: 0,
                parcialI: 35m,
                parcialII: 30m,
                puntajeTareaCurso: 20m,
                idInscripcion: 8,
                nombre: "Pablo")
        };
        _consulta
            .Setup(x => x.ObtenerPorCursoAsync(9, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resumenes);

        var filas = await _sut.ObtenerAsync(9, CancellationToken.None);

        Assert.Equal(3, filas.Count);
        Assert.Equal(ResultadoAcademico.Retirado, filas[0].ResultadoAcademico);
        Assert.Equal(ResultadoAcademico.EnCurso, filas[1].ResultadoAcademico);
        Assert.Equal(ResultadoAcademico.Aprobado, filas[2].ResultadoAcademico);
        Assert.Equal(65m, filas[2].NotaFinal);
    }

    [Fact]
    public async Task ObtenerAsync_CuandoNoHayInscripciones_DevuelveListaVacia()
    {
        _consulta
            .Setup(x => x.ObtenerPorCursoAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<ResumenSituacionEstudiante>());

        var filas = await _sut.ObtenerAsync(99, CancellationToken.None);

        Assert.Empty(filas);
    }

    private void ConfigurarConsulta(int idCursoOfertado, ResumenSituacionEstudiante resumen)
    {
        _consulta
            .Setup(x => x.ObtenerPorCursoAsync(idCursoOfertado, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { resumen });
    }

    private void VerificarConsulta(int idCursoOfertado)
    {
        _consulta.Verify(
            x => x.ObtenerPorCursoAsync(idCursoOfertado, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
