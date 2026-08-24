using GestionAcademica.Api.Reportes.Aplicacion;

namespace GestionAcademica.Api.Tests.Reportes;

internal static class ResumenSituacionFactory
{
    public static ResumenSituacionEstudiante Crear(
        string estadoInscripcion = ResultadoAcademico.EstadoInscripcionActiva,
        int evaluacionesPendientes = 0,
        decimal parcialI = 30m,
        decimal parcialII = 35m,
        decimal? puntajeTareaCurso = 20m,
        int idInscripcion = 1,
        int idEstudiante = 1,
        string carnet = "E2024-001",
        string nombre = "Estudiante de prueba")
    {
        return new ResumenSituacionEstudiante(
            idInscripcion,
            idEstudiante,
            carnet,
            nombre,
            estadoInscripcion,
            evaluacionesPendientes,
            parcialI,
            parcialII,
            puntajeTareaCurso);
    }
}
