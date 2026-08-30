using GestionAcademica.Api.Inscripciones.Aplicacion;
using GestionAcademica.Api.Reportes.Aplicacion;

namespace GestionAcademica.Api.Tests.Reportes;

public sealed class ReglasReporteCalificacionesTests
{
    [Fact]
    public void EstadoRetirada_EsElMismoTextoQueInscripciones()
    {
        Assert.Equal("Retirada", ReglasReporteCalificaciones.EstadoRetirada);
        Assert.Equal(ReglasInscripcion.Retirada, ReglasReporteCalificaciones.EstadoRetirada);
        Assert.NotEqual("Retirado", ReglasReporteCalificaciones.EstadoRetirada);
    }

    [Fact]
    public void DebePonerCeros_CuandoRetiradaSinNotas_EsVerdadero()
    {
        Assert.True(ReglasReporteCalificaciones.DebePonerCeros(
            ReglasInscripcion.Retirada,
            new decimal?[] { null, null }));
    }

    [Fact]
    public void DebePonerCeros_CuandoRetiradaSoloCerosOSinEvaluar_EsVerdadero()
    {
        Assert.True(ReglasReporteCalificaciones.DebePonerCeros(
            ReglasInscripcion.Retirada,
            new decimal?[] { 0m, null, 0m }));
    }

    [Theory]
    [InlineData("Retirada")]
    [InlineData("retirada")]
    [InlineData("RETIRADA")]
    public void DebePonerCeros_CuandoRetiradaIgnoraMayusculas(string estado)
    {
        Assert.True(ReglasReporteCalificaciones.DebePonerCeros(estado, new decimal?[] { null }));
    }

    [Fact]
    public void DebePonerCeros_CuandoRetiradaConNotaReal_EsFalso()
    {
        Assert.False(ReglasReporteCalificaciones.DebePonerCeros(
            ReglasInscripcion.Retirada,
            new decimal?[] { 7m, null, 0m }));
    }

    [Theory]
    [InlineData(ReglasInscripcion.Activa)]
    [InlineData(ReglasInscripcion.Pendiente)]
    public void DebePonerCeros_CuandoNoEstaRetirada_EsFalso(string estado)
    {
        Assert.False(ReglasReporteCalificaciones.DebePonerCeros(
            estado,
            new decimal?[] { null, 0m }));
    }
}
