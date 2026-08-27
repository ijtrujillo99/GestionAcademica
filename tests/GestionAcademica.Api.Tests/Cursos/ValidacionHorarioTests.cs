using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Cursos.Aplicacion;

namespace GestionAcademica.Api.Tests.Cursos;

public sealed class ValidacionHorarioTests
{
    [Fact]
    public void Validar_CuandoDiaEstaFueraDeRango_Lanza()
    {
        Assert.Throws<ReglaNegocioException>(() =>
            ValidacionHorario.Validar(0, TimeSpan.FromHours(8), TimeSpan.FromHours(10)));
    }

    [Fact]
    public void Validar_CuandoHoraFinNoEsPosterior_Lanza()
    {
        Assert.Throws<ReglaNegocioException>(() =>
            ValidacionHorario.Validar(2, TimeSpan.FromHours(10), TimeSpan.FromHours(10)));
    }

    [Fact]
    public void Validar_CuandoElBloqueEsCoherente_NoLanza()
    {
        ValidacionHorario.Validar(2, new TimeSpan(12, 40, 0), new TimeSpan(14, 20, 0));
    }
}
