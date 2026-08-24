namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Horario
{
    public int IdHorario { get; set; }
    public int IdCursoOfertado { get; set; }
    public byte DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }

    public CursoOfertado CursoOfertado { get; set; } = null!;
    public ICollection<Clase> Clases { get; set; } = new List<Clase>();
}
