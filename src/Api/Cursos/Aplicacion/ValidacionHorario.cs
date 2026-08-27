using GestionAcademica.Api.Comun;

namespace GestionAcademica.Api.Cursos.Aplicacion;

public static class ValidacionHorario
{
    public static void Validar(byte diaSemana, TimeSpan horaInicio, TimeSpan horaFin)
    {
        if (diaSemana is < 1 or > 7)
        {
            throw new ReglaNegocioException(
                "El día de la semana debe estar entre 1 (lunes) y 7 (domingo).");
        }

        if (horaFin <= horaInicio)
        {
            throw new ReglaNegocioException(
                "La hora de fin debe ser posterior a la de inicio.");
        }
    }
}
