using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Inscripciones.Aplicacion;

namespace GestionAcademica.Api.Tests.Inscripciones;

public sealed class ReglasInscripcionTests
{
    [Fact]
    public void AplicarEstado_CuandoPasaARetiradaYNoHabiaFecha_PoneHoy()
    {
        var inicio = new DateTime(2026, 8, 4);
        var ahora = new DateTime(2026, 8, 24, 15, 0, 0);

        var (fechaRetiro, fechaEstado) = ReglasInscripcion.AplicarEstado(
            ReglasInscripcion.Retirada,
            inicio,
            fechaRetiroActual: null,
            ahora);

        Assert.Equal(ahora.Date, fechaRetiro);
        Assert.Equal(ahora, fechaEstado);
    }

    [Fact]
    public void AplicarEstado_CuandoYaEstabaRetirada_ConservaFechaRetiro()
    {
        var inicio = new DateTime(2026, 8, 4);
        var retiro = new DateTime(2026, 8, 7);
        var ahora = new DateTime(2026, 8, 24, 15, 0, 0);

        var (fechaRetiro, _) = ReglasInscripcion.AplicarEstado(
            ReglasInscripcion.Retirada,
            inicio,
            retiro,
            ahora);

        Assert.Equal(retiro, fechaRetiro);
    }

    [Fact]
    public void AplicarEstado_CuandoVuelveAActiva_LimpiaFechaRetiro()
    {
        var inicio = new DateTime(2026, 8, 4);
        var retiro = new DateTime(2026, 8, 7);
        var ahora = new DateTime(2026, 8, 24, 15, 0, 0);

        var (fechaRetiro, fechaEstado) = ReglasInscripcion.AplicarEstado(
            ReglasInscripcion.Activa,
            inicio,
            retiro,
            ahora);

        Assert.Null(fechaRetiro);
        Assert.Equal(ahora, fechaEstado);
    }

    [Fact]
    public void AplicarEstado_CuandoPasaAPendiente_LimpiaFechaRetiro()
    {
        var inicio = new DateTime(2026, 8, 4);
        var retiro = new DateTime(2026, 8, 7);
        var ahora = new DateTime(2026, 8, 24, 15, 0, 0);

        var (fechaRetiro, _) = ReglasInscripcion.AplicarEstado(
            ReglasInscripcion.Pendiente,
            inicio,
            retiro,
            ahora);

        Assert.Null(fechaRetiro);
    }

    [Fact]
    public void AplicarEstado_CuandoRetiroEsAnteriorAlInicio_Lanza()
    {
        var inicio = new DateTime(2026, 8, 10);
        var ahora = new DateTime(2026, 8, 4, 10, 0, 0);

        var error = Assert.Throws<ReglaNegocioException>(() =>
            ReglasInscripcion.AplicarEstado(
                ReglasInscripcion.Retirada,
                inicio,
                fechaRetiroActual: null,
                ahora));

        Assert.Contains("retiro", error.Message, StringComparison.OrdinalIgnoreCase);
    }
}
