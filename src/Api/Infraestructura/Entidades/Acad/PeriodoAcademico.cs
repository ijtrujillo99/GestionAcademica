namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class PeriodoAcademico
{
    public int IdPeriodo { get; set; }
    public short Anio { get; set; }
    public byte Semestre { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
