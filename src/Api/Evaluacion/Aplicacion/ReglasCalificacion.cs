using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Inscripciones.Aplicacion;

namespace GestionAcademica.Api.ModuloEvaluacion.Aplicacion;

public static class ReglasCalificacion
{
    public static void AsegurarAltaPermitida(
        int idInscripcion,
        string estadoInscripcion,
        bool existeCalificacion)
    {
        if (ReglasInscripcion.EsRetirada(estadoInscripcion) && !existeCalificacion)
        {
            throw new ReglaNegocioException(
                $"La inscripción {idInscripcion} está retirada y no puede recibir una nueva calificación.");
        }
    }
}
