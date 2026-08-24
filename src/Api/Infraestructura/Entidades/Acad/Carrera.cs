namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Carrera
{
    public int IdCarrera { get; set; }
    public int IdAreaConocimiento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public AreaConocimiento AreaConocimiento { get; set; } = null!;
    public ICollection<PlanEstudio> PlanesEstudio { get; set; } = new List<PlanEstudio>();
}
