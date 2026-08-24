namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Docente
{
    public int IdDocente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<CursoOfertado> CursosOfertados { get; set; } = new List<CursoOfertado>();
}
