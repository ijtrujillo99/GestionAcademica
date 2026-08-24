namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Grupo
{
    public int IdGrupo { get; set; }
    public int IdPeriodo { get; set; }
    public string CodigoGrupo { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public PeriodoAcademico Periodo { get; set; } = null!;
    public ICollection<CursoOfertado> CursosOfertados { get; set; } = new List<CursoOfertado>();
}
