using GestionAcademica.Api.Infraestructura.Entidades.Eval;

namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class CursoOfertado
{
    public int IdCursoOfertado { get; set; }
    public int IdAsignatura { get; set; }
    public int IdGrupo { get; set; }
    public int IdDocente { get; set; }
    public int IdAula { get; set; }
    public short? CupoMaximo { get; set; }
    public bool Activo { get; set; } = true;

    public Asignatura Asignatura { get; set; } = null!;
    public Grupo Grupo { get; set; } = null!;
    public Docente Docente { get; set; } = null!;
    public Aula Aula { get; set; } = null!;
    public ICollection<Horario> Horarios { get; set; } = new List<Horario>();
    public ICollection<InscripcionCurso> Inscripciones { get; set; } = new List<InscripcionCurso>();
    public ICollection<Clase> Clases { get; set; } = new List<Clase>();
    public ICollection<Parcial> Parciales { get; set; } = new List<Parcial>();
}
