namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class AreaConocimiento
{
    public int IdAreaConocimiento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Carrera> Carreras { get; set; } = new List<Carrera>();
}
