using GestionAcademica.Api.Infraestructura.Entidades.Acad;

namespace GestionAcademica.Api.Infraestructura.Entidades.Eval;

public class Calificacion
{
    public int IdCalificacion { get; set; }
    public int IdEvaluacion { get; set; }
    public int IdInscripcion { get; set; }
    public decimal PuntajeObtenido { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string? Observacion { get; set; }

    public Evaluacion Evaluacion { get; set; } = null!;
    public InscripcionCurso Inscripcion { get; set; } = null!;
}
