using GestionAcademica.Api.Reportes.Aplicacion;

namespace GestionAcademica.Api.Tests.Reportes;

public sealed class ClasificadorResultadoAcademicoTests
{
    private readonly ClasificadorResultadoAcademico _sut = new();

    [Theory]
    [InlineData("Retirada", 0, 30, 30, 10.0)]
    [InlineData("retirada", 3, 0, 0, 0.0)]
    [InlineData("RETIRADA", 1, 50, 50, null)]
    public void Clasificar_CuandoInscripcionEstaRetirada_DevuelveRetirado(
        string estadoInscripcion,
        int evaluacionesPendientes,
        double parcialI,
        double parcialII,
        double? puntajeTareaCurso)
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: estadoInscripcion,
            evaluacionesPendientes: evaluacionesPendientes,
            parcialI: (decimal)parcialI,
            parcialII: (decimal)parcialII,
            puntajeTareaCurso: (decimal?)puntajeTareaCurso);

        var resultado = _sut.Clasificar(resumen);

        Assert.Equal(ResultadoAcademico.Retirado, resultado);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(8)]
    public void Clasificar_CuandoHayEvaluacionesPendientes_DevuelveEnCurso(
        int evaluacionesPendientes)
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: evaluacionesPendientes,
            parcialI: 35m,
            parcialII: 30m,
            puntajeTareaCurso: 20m);

        var resultado = _sut.Clasificar(resumen);

        Assert.Equal(ResultadoAcademico.EnCurso, resultado);
        Assert.True(resumen.NotaFinal >= ResultadoAcademico.NotaMinimaAprobacion);
    }

    [Fact]
    public void Clasificar_CuandoEstadoEsPendienteYHayEvaluacionesPendientes_DevuelveEnCurso()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: "Pendiente",
            evaluacionesPendientes: 2,
            parcialI: 0m,
            parcialII: 0m,
            puntajeTareaCurso: null);

        var resultado = _sut.Clasificar(resumen);

        Assert.Equal(ResultadoAcademico.EnCurso, resultado);
    }

    [Theory]
    [InlineData(29.99, 30.00, 10.0, ResultadoAcademico.Reprobado)]
    [InlineData(30.00, 30.00, 10.0, ResultadoAcademico.Aprobado)]
    [InlineData(30.00, 30.01, 10.0, ResultadoAcademico.Aprobado)]
    public void Clasificar_CuandoNoHayPendientes_ClasificaPorUmbralDeSesenta(
        double parcialI,
        double parcialII,
        double puntajeTareaCurso,
        string resultadoEsperado)
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 0,
            parcialI: (decimal)parcialI,
            parcialII: (decimal)parcialII,
            puntajeTareaCurso: (decimal)puntajeTareaCurso);

        var resultado = _sut.Clasificar(resumen);

        Assert.Equal((decimal)parcialI + (decimal)parcialII, resumen.NotaFinal);
        Assert.Equal(resultadoEsperado, resultado);
    }

    [Fact]
    public void Clasificar_CuandoTareaYaEstaEnParcialII_NoLaSumaOtraVez()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 0,
            parcialI: 35m,
            parcialII: 30m,
            puntajeTareaCurso: 20m);

        var resultado = _sut.Clasificar(resumen);

        Assert.Equal(65m, resumen.NotaFinal);
        Assert.Equal(20m, resumen.PuntajeTareaCurso);
        Assert.Equal(ResultadoAcademico.Aprobado, resultado);
    }

    [Theory]
    [InlineData(35, 30, 0.0)]
    [InlineData(40, 20, 0.0)]
    [InlineData(30, 30, 0.0)]
    public void Clasificar_CuandoAlcanzaSesentaPeroTareaValeCero_DevuelveNoAprobadoTareaCurso(
        double parcialI,
        double parcialII,
        double puntajeTareaCurso)
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 0,
            parcialI: (decimal)parcialI,
            parcialII: (decimal)parcialII,
            puntajeTareaCurso: (decimal)puntajeTareaCurso);

        var resultado = _sut.Clasificar(resumen);

        Assert.True(resumen.NotaFinal >= ResultadoAcademico.NotaMinimaAprobacion);
        Assert.Equal(0m, resumen.PuntajeTareaCurso);
        Assert.Equal(ResultadoAcademico.NoAprobadoTareaCurso, resultado);
    }

    [Fact]
    public void Clasificar_CuandoNoHayPendientesNiTareaCalificadaYNotaAlcanza_DevuelveEnCurso()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionActiva,
            evaluacionesPendientes: 0,
            parcialI: 30m,
            parcialII: 30m,
            puntajeTareaCurso: null);

        var resultado = _sut.Clasificar(resumen);

        Assert.Equal(60m, resumen.NotaFinal);
        Assert.Equal(ResultadoAcademico.EnCurso, resultado);
    }

    [Fact]
    public void Clasificar_CuandoInscripcionRetirada_TienePrioridadSobrePendientesYNotas()
    {
        var resumen = ResumenSituacionFactory.Crear(
            estadoInscripcion: ResultadoAcademico.EstadoInscripcionRetirada,
            evaluacionesPendientes: 4,
            parcialI: 50m,
            parcialII: 50m,
            puntajeTareaCurso: 0m);

        var resultado = _sut.Clasificar(resumen);

        Assert.Equal(ResultadoAcademico.Retirado, resultado);
    }
}
