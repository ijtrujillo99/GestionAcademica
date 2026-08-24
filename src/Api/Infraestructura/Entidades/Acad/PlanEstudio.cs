namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class PlanEstudio
{
    public int IdPlanEstudio { get; set; }
    public int IdCarrera { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public short AnioInicio { get; set; }
    public bool Activo { get; set; } = true;

    public Carrera Carrera { get; set; } = null!;
    public ICollection<PlanEstudioDetalle> Detalles { get; set; } = new List<PlanEstudioDetalle>();
}
