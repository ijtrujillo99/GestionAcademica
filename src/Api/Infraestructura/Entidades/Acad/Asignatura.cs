namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Asignatura
{
    public int IdAsignatura { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<PlanEstudioDetalle> Planes { get; set; } = new List<PlanEstudioDetalle>();
    public ICollection<CursoOfertado> CursosOfertados { get; set; } = new List<CursoOfertado>();
}
