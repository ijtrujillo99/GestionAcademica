namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Clase
{
    public int IdClase { get; set; }
    public int IdCursoOfertado { get; set; }
    public int? IdHorario { get; set; }
    public DateTime Fecha { get; set; }
    public string? Tema { get; set; }
    public string? Descripcion { get; set; }

    public CursoOfertado CursoOfertado { get; set; } = null!;
    public Horario? Horario { get; set; }
    public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
}
