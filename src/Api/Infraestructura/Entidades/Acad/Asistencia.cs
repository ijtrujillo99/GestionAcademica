using GestionAcademica.Api.Infraestructura.Entidades.Cat;

namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Asistencia
{
    public int IdAsistencia { get; set; }
    public int IdClase { get; set; }
    public int IdInscripcion { get; set; }
    public int IdEstadoAsistencia { get; set; }
    public string? Observacion { get; set; }

    public Clase Clase { get; set; } = null!;
    public InscripcionCurso Inscripcion { get; set; } = null!;
    public EstadoAsistencia EstadoAsistencia { get; set; } = null!;
}
