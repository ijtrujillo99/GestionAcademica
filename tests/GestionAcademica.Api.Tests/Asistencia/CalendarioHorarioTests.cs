using System.Globalization;
using GestionAcademica.Api.ModuloAsistencia.Aplicacion;

namespace GestionAcademica.Api.Tests.Asistencia;

public sealed class CalendarioHorarioTests
{
    [Theory]
    [InlineData("2026-08-24", 1)]
    [InlineData("2026-08-25", 2)]
    [InlineData("2026-08-26", 3)]
    [InlineData("2026-08-27", 4)]
    [InlineData("2026-08-28", 5)]
    [InlineData("2026-08-29", 6)]
    [InlineData("2026-08-23", 7)]
    public void DiaSemana_ConvierteSegunCalendarioSql(string fecha, byte esperado)
    {
        var valor = DateTime.Parse(fecha, CultureInfo.InvariantCulture);

        Assert.Equal(esperado, CalendarioHorario.DiaSemana(valor));
    }
}
