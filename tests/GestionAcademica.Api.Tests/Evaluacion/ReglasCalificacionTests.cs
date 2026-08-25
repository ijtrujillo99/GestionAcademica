using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Inscripciones.Aplicacion;
using GestionAcademica.Api.ModuloEvaluacion.Aplicacion;

namespace GestionAcademica.Api.Tests.Evaluacion;

public sealed class ReglasCalificacionTests
{
    [Fact]
    public void AsegurarAltaPermitida_CuandoActivaSinCalificacion_NoLanza()
    {
        ReglasCalificacion.AsegurarAltaPermitida(6, ReglasInscripcion.Activa, existeCalificacion: false);
    }

    [Fact]
    public void AsegurarAltaPermitida_CuandoActivaConCalificacion_NoLanza()
    {
        ReglasCalificacion.AsegurarAltaPermitida(6, ReglasInscripcion.Activa, existeCalificacion: true);
    }

    [Fact]
    public void AsegurarAltaPermitida_CuandoPendienteSinCalificacion_NoLanza()
    {
        ReglasCalificacion.AsegurarAltaPermitida(6, ReglasInscripcion.Pendiente, existeCalificacion: false);
    }

    [Fact]
    public void AsegurarAltaPermitida_CuandoRetiradaSinCalificacion_Lanza()
    {
        var error = Assert.Throws<ReglaNegocioException>(() =>
            ReglasCalificacion.AsegurarAltaPermitida(6, ReglasInscripcion.Retirada, existeCalificacion: false));

        Assert.Equal(
            "La inscripción 6 está retirada y no puede recibir una nueva calificación.",
            error.Message);
    }

    [Fact]
    public void AsegurarAltaPermitida_CuandoRetiradaConCalificacion_NoLanza()
    {
        ReglasCalificacion.AsegurarAltaPermitida(6, ReglasInscripcion.Retirada, existeCalificacion: true);
    }
}
