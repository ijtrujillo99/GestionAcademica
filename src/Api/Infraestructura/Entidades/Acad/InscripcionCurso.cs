using GestionAcademica.Api.Infraestructura.Entidades.Cat;
using GestionAcademica.Api.Infraestructura.Entidades.Eval;

namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class InscripcionCurso
{
    public int IdInscripcion { get; set; }
    public int IdEstudiante { get; set; }
    public int IdCursoOfertado { get; set; }
    public int IdEstadoInscripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaEstado { get; set; }
    public DateTime? FechaRetiro { get; set; }
    public bool EsOficial { get; set; } = true;
    public string? Observacion { get; set; }

    public Estudiante Estudiante { get; set; } = null!;
    public CursoOfertado CursoOfertado { get; set; } = null!;
    public EstadoInscripcion EstadoInscripcion { get; set; } = null!;
    public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
    public ICollection<Calificacion> Calificaciones { get; set; } = new List<Calificacion>();
}
