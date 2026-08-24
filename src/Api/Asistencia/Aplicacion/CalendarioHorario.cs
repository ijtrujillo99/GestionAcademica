namespace GestionAcademica.Api.ModuloAsistencia.Aplicacion;

public static class CalendarioHorario
{
    public static byte DiaSemana(DateTime fecha)
    {
        return fecha.DayOfWeek == DayOfWeek.Sunday
            ? (byte)7
            : (byte)fecha.DayOfWeek;
    }
}
