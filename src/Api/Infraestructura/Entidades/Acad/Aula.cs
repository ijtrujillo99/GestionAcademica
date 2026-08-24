namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Aula
{
    public int IdAula { get; set; }
    public string CodigoAula { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public short? Capacidad { get; set; }
    public string? Ubicacion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<CursoOfertado> CursosOfertados { get; set; } = new List<CursoOfertado>();
}
