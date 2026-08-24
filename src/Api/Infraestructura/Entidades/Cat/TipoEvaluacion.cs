using GestionAcademica.Api.Infraestructura.Entidades.Eval;

namespace GestionAcademica.Api.Infraestructura.Entidades.Cat;

public class TipoEvaluacion
{
    public int IdTipoEvaluacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();
}
